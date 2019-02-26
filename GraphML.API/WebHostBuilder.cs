using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using GraphML.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace GraphML.API
{
  internal static class WebHostBuilder
  {
    public static IWebHost BuildWebHost(string[] args)
    {
      var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile("hosting.json")
        .AddEnvironmentVariables()
        .AddUserSecrets<Program>()
        .Build();

      return WebHost.CreateDefaultBuilder(args)
        .UseStartup<Startup>()
        .UseKestrel(options =>
        {
          var httpsInfos = new Dictionary<int, IPAddress>();
          var allUrls = (Settings.KESTREL_URLS(config)).Split(';');
          foreach (var url in allUrls)
          {
            var updatedUrl = url;
            if (updatedUrl.Contains("*"))
            {
              updatedUrl = updatedUrl.Replace("*", "0.0.0.0");
            }

            var listenUrl = new Uri(updatedUrl);
            if (listenUrl.Scheme == "https")
            {
              httpsInfos.Add(listenUrl.Port, IPAddress.Parse(listenUrl.Host));
              continue;
            }
            options.Listen(listenUrl.IsLoopback ? IPAddress.Loopback : IPAddress.Any, listenUrl.Port);
          }

          var certFileName = Settings.KESTREL_CERTIFICATE_FILENAME(config);
          if (!string.IsNullOrEmpty(certFileName) && File.Exists(certFileName))
          {
            var httpsPort = Settings.KESTREL_HTTPS_PORT(config);
            httpsInfos.Add(httpsPort, IPAddress.Any);

            foreach (var kvp in httpsInfos)
            {
              options.Listen(kvp.Value, kvp.Key, listenOptions =>
            {
                var certPassword = Settings.KESTREL_CERTIFICATE_PASSWORD(config);
                listenOptions.UseHttps(certFileName, certPassword);
              });
            }
          }
        })
        .ConfigureServices(services => services.AddAutofac())
        .UseConfiguration(config)
        .ConfigureLogging(logging =>
        {
          logging.SetMinimumLevel(LogLevel.Trace);
        })
        .UseNLog()  // NLog: setup NLog for Dependency injection
        .Build();
    }
  }
}

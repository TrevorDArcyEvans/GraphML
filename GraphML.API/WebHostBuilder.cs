using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
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
        .AddJsonFile("hosting.json")
        .Build();

      return WebHost.CreateDefaultBuilder(args)
        .UseStartup<Startup>()
        .UseKestrel(options =>
        {
          var httpsInfos = new Dictionary<int, IPAddress>();
          var allUrls = (config.GetValue<string>("urls") ?? "http://localhost:5000").Split(';');
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

          var certCfg = config.GetSection("Certificate");
          var certFileName = certCfg.GetValue<string>("FileName") ?? "GraphML.pfx";
          if (!string.IsNullOrEmpty(certFileName))
          {
            var httpsPort = certCfg.GetValue<int>(Constants.HttpsPortKey) == 0 ? 8000 : certCfg.GetValue<int>(Constants.HttpsPortKey);
            httpsInfos.Add(httpsPort, IPAddress.Any);

            foreach (var kvp in httpsInfos)
            {
              options.Listen(kvp.Value, kvp.Key, listenOptions =>
              {
                var certPassword = certCfg.GetValue<string>("Password") ?? "DisruptTheMarket";
                listenOptions.UseHttps(certFileName, certPassword);
              });
            }
          }
        })
        .UseConfiguration(config)
        .Build();
    }
  }
}

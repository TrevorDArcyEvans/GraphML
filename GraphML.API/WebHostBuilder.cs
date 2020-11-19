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
        .UseKestrel()
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

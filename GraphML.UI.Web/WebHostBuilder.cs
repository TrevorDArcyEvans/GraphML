using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace GraphML.UI.Web
{
  internal static class WebHostBuilder
  {
    public static IWebHost BuildWebHost(string[] args)
    {
      var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile("hosting.json")
        .AddJsonFile($"autofac.json", optional: true)
        .AddEnvironmentVariables()
        .AddUserSecrets<Program>()
        .Build();

      return WebHost.CreateDefaultBuilder(args)
        .UseStartup<Startup>()
        .UseKestrel()
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

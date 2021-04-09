using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace GraphML.API
{
  internal static class WebHostBuilder
  {
    public static IWebHost BuildWebHost(string[] args)
    {
      return WebHost.CreateDefaultBuilder(args)
        .UseStartup<Startup>()
        .UseKestrel()
        .ConfigureServices(services => services.AddAutofac())
        .ConfigureLogging(logging => { logging.SetMinimumLevel(LogLevel.Trace); })
        .UseNLog() // NLog: setup NLog for Dependency injection
        .Build();
    }
  }
}

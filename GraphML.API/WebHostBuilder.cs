using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace GraphML.API
{
  internal static class WebHostBuilder
  {
    public static IWebHostBuilder BuildWebHost(string[] args)
    {
      return WebHost.CreateDefaultBuilder(args)
        .ConfigureKestrel(options =>
        {
          // TODO   get gRPC port from config
          options.ListenAnyIP(5020, listenOptions => listenOptions.Protocols = HttpProtocols.Http2);
        })
        .UseStartup<Startup>()
        .ConfigureServices(services => services.AddAutofac())
        .ConfigureLogging(logging => { logging.SetMinimumLevel(LogLevel.Trace); })
        .UseNLog(); // NLog: setup NLog for Dependency injection
    }
  }
}

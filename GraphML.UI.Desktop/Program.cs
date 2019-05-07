using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using System;
using System.Windows.Forms;

namespace GraphML.UI.Desktop
{
  static class Program
  {
    [STAThread]
    static void Main()
    {
      try
      {
        NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration("NLog.config");

        var builder = new ConfigurationBuilder()
          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
          .AddJsonFile("hosting.json", optional: true, reloadOnChange: true)
          .AddJsonFile($"autofac.json", optional: true);
        var config = builder.Build();

        // database connection string for NLog
        GlobalDiagnosticsContext.Set("LOG_CONNECTIONSTRING", Settings.LOG_CONNECTIONSTRING(config));

        using (var sp = BuildDi(config))
        {
          Application.EnableVisualStyles();
          Application.SetCompatibleTextRenderingDefault(false);

          Application.Run(new Main(sp));
        }
      }
      finally
      {
        // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
        NLog.LogManager.Shutdown();
      }
    }

    private static ServiceProvider BuildDi(IConfiguration config)
    {
      return new ServiceCollection()
        .AddSingleton(config)
        .AddSingleton<IRestClientFactory, RestClientFactory>()
        .AddSingleton<ISyncPolicyFactory, SyncPolicyFactory>()
        .AddSingleton<IRepositoryManagerServer, RepositoryManagerServer>()
        .AddSingleton<IRepositoryServer, RepositoryServer>()
        .AddLogging(loggingBuilder =>
        {
          // configure Logging with NLog
          loggingBuilder.ClearProviders();
          loggingBuilder.SetMinimumLevel(LogLevel.Trace);
          loggingBuilder.AddNLog(config);
        })
        .BuildServiceProvider();
    }
  }
}

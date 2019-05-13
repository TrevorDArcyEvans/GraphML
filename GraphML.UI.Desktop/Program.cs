using Autofac;
using Autofac.Configuration;
using Autofac.Extensions.DependencyInjection;
using GraphML.Common;
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
        using (var sp = BuildServiceProvider())
        {
          var config = sp.GetRequiredService<IConfiguration>();

          NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration("NLog.config");

          // database connection string for NLog
          GlobalDiagnosticsContext.Set("LOG_CONNECTIONSTRING", Settings.LOG_CONNECTION_STRING(config));

          Application.EnableVisualStyles();
          Application.SetCompatibleTextRenderingDefault(false);

          var main = sp.GetRequiredService<Main>();
          Application.Run(main);
        }
      }
      finally
      {
        // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
        NLog.LogManager.Shutdown();
      }
    }

    private static AutofacServiceProvider BuildServiceProvider()
    {
      var cfgBuilder = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile("hosting.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"autofac.json", optional: true);
      var config = cfgBuilder.Build();

      var services = new ServiceCollection()
        .AddSingleton<IConfiguration>(config)
        .AddLogging(loggingBuilder =>
        {
          // configure Logging with NLog
          loggingBuilder.ClearProviders();
          loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
          loggingBuilder.AddNLog(config);
        });

      // Create the container builder.
      var contBuilder = new ContainerBuilder();

      // Register dependencies, populate the services from
      // the collection, and build the container.
      //
      // Note that Populate is basically a foreach to add things
      // into Autofac that are in the collection. If you register
      // things in Autofac BEFORE Populate then the stuff in the
      // ServiceCollection can override those things; if you register
      // AFTER Populate those registrations can override things
      // in the ServiceCollection. Mix and match as needed.
      contBuilder.Populate(services);

      //load configuration from autofac.json
      var module = new ConfigurationModule(config);
      contBuilder.RegisterModule(module);

      var appCont = contBuilder.Build();

      // Create the IServiceProvider based on the container.
      return new AutofacServiceProvider(appCont);
    }
  }
}

using Autofac;
using Autofac.Extensions.DependencyInjection;
using GraphML.Common;
using GraphML.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading;

namespace GraphML.Analysis.Server
{
  public sealed class Program
  {
    private static readonly object _lock = new object();
    private IServiceProvider ServiceProvider { get; set; }
    private IRequestMessageReceiver _receiver;

    public static void Main(string[] args)
    {
      try
      {
        new Program().Run(args);
      }
      finally
      {
        // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
        NLog.LogManager.Shutdown();
      }
    }

    private void Run(string[] args)
    {
      AssemblyLoadContext.Default.Resolving += OnAssemblyResolve;

      var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile("hosting.json")
        .AddEnvironmentVariables()
        .AddUserSecrets<Program>()
        .Build();
      Settings.DumpSettings(config);

      // database connection string for nLog
      GlobalDiagnosticsContext.Set("LOG_CONNECTION_STRING", Settings.LOG_CONNECTION_STRING(config));

      // The Microsoft.Extensions.DependencyInjection.ServiceCollection
      // has extension methods provided by other .NET Core libraries to
      // regsiter services with DI.
      var serviceCollection = new ServiceCollection();

      // The Microsoft.Extensions.Logging package provides this one-liner
      // to add logging services.
      serviceCollection.AddLogging();

      // Create the container builder.
      var containerBuilder = new ContainerBuilder();

      // Once you've registered everything in the ServiceCollection, call
      // Populate to bring those registrations into Autofac. This is
      // just like a foreach over the list of things in the collection
      // to add them to Autofac.
      containerBuilder.Populate(serviceCollection);

      // load all assemblies in same directory and register classes with interfaces
      // Note that we have to explicitly add this (executing) assembly
      var exeAssy = Assembly.GetExecutingAssembly();
      var exeAssyPath = exeAssy.Location;
      var exeAssyDir = Path.GetDirectoryName(exeAssyPath);
      var assyDllPaths = Directory.EnumerateFiles(exeAssyDir, "GraphML.*.dll");
      var assyPaths = new List<string>();
      assyPaths.AddRange(assyDllPaths);

      var assys = assyPaths.Select(filePath => Assembly.LoadFrom(filePath)).ToList();
      assys.Add(exeAssy);
      containerBuilder
        .RegisterAssemblyTypes(assys.ToArray())
        .PublicOnly()
        .AsImplementedInterfaces()
        .SingleInstance();

      containerBuilder
        .Register(cc => config)
        .As<IConfiguration>();

      // Create Logger<T> when ILogger<T> is required.
      containerBuilder
        .RegisterGeneric(typeof(Logger<>))
        .As(typeof(ILogger<>));

      // Use NLogLoggerFactory as a factory required by Logger<T>.
      containerBuilder
        .RegisterType<NLogLoggerFactory>()
        .AsImplementedInterfaces()
        .SingleInstance();

      // Create the IServiceProvider based on the container.
      var container = containerBuilder.Build();
      ServiceProvider = new AutofacServiceProvider(container);

      _receiver = ServiceProvider.GetRequiredService<IRequestMessageReceiver>();

      while (true)
      {
        if (Settings.MESSAGE_QUEUE_USE_THREADS(config))
        {
          ThreadPool.QueueUserWorkItem(x => { DoMessageLoop(); });
        }
        else
        {
          DoMessageLoop();
        }
      }
    }

    private void DoMessageLoop()
    {
      var json = _receiver.Receive();
      if (json != null)
      {
        ProcessMessage(json);
      }
    }

    private void ProcessMessage(string json)
    {
      var jobj = JObject.Parse(json);
      var reqTypeStr = jobj["Type"].ToString();
      var reqType = Type.GetType(reqTypeStr);
      var req = JsonConvert.DeserializeObject(json, reqType);
      var jobType = Type.GetType(((RequestBase)req).JobType);
      var job = (IJob)ServiceProvider.GetService(jobType);

      job.Run((RequestBase)req);
    }

    private Assembly OnAssemblyResolve(AssemblyLoadContext assemblyLoadContext, AssemblyName assemblyName)
    {
      lock (_lock)
      {
        AssemblyLoadContext.Default.Resolving -= OnAssemblyResolve;
        try
        {
          var currAssyPath = Assembly.GetExecutingAssembly().Location;
          var assyPath = Path.Combine(Path.GetDirectoryName(currAssyPath), $"{assemblyName.Name}.dll");
          var assembly = File.Exists(assyPath) ? Assembly.LoadFile(assyPath) : null;
          return assembly;
        }
        finally
        {
          AssemblyLoadContext.Default.Resolving += OnAssemblyResolve;
        }
      }
    }
  }
}

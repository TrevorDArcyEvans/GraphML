using Autofac;
using Autofac.Extensions.DependencyInjection;
using GraphML.Common;
using GraphML.Interfaces;
using Microsoft.AspNetCore.Http;
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
using Dapper;

namespace GraphML.Analysis.Server
{
  public sealed class Program
  {
    private static readonly object _lock = new object();
    private IServiceProvider _serviceProvider;
    private IRequestMessageReceiver _receiver;
    private ILogger<Program> _logger;

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
        .AddJsonFile("appsettings.development.json", optional: true, reloadOnChange: true)
        .AddJsonFile("hosting.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables()
        .AddUserSecrets<Program>()
        .Build();
      Settings.DumpSettings(config);

      // database connection string for nLog
      GlobalDiagnosticsContext.Set("LOG_CONNECTION_STRING", config.LOG_CONNECTION_STRING());

      SqlMapper.AddTypeHandler(new GuidTypeHandler());
      SqlMapper.RemoveTypeMap(typeof(Guid));
      SqlMapper.RemoveTypeMap(typeof(Guid?));

      // The Microsoft.Extensions.DependencyInjection.ServiceCollection
      // has extension methods provided by other .NET Core libraries to
      // regsiter services with DI.
      var serviceCollection = new ServiceCollection();

      serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

      // The Microsoft.Extensions.Logging package provides this one-liner
      // to add logging services.
      serviceCollection.AddLogging();

#region Autofac

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
      _serviceProvider = new AutofacServiceProvider(container);

#endregion

      _receiver = _serviceProvider.GetRequiredService<IRequestMessageReceiver>();
      _logger = _serviceProvider.GetRequiredService<ILogger<Program>>();

      while (true)
      {
        if (config.MESSAGE_QUEUE_USE_THREADS())
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
      if (!string.IsNullOrWhiteSpace(json))
      {
        ProcessMessage(json);
      }
    }

    private void ProcessMessage(string json)
    {
      try
      {
        var jobj = JObject.Parse(json);
        var reqTypeStr = jobj["Type"].ToString();
        var reqType = Type.GetType(reqTypeStr);
        var req = (IRequest)JsonConvert.DeserializeObject(json, reqType);
        var jobType = Type.GetType(req.JobType);
        var job = (IJob)_serviceProvider.GetService(jobType);

        job.Run(req);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, $"{ex.Message} --> {json}");
      }
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

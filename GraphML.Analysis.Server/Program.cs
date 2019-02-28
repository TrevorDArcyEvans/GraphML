using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.Util;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using GraphML.Analysis.SNA.Centrality;
using GraphML.Common;
using GraphML.Datastore.Database;
using GraphML.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

    private IConfiguration Configuration { get; set; }
    private IServiceProvider ServiceProvider { get; set; }

    public static void Main(string[] args)
    {
      new Program().Run(args);
    }

    private void Run(string[] args)
    {
      AssemblyLoadContext.Default.Resolving += OnAssemblyResolve;

      Configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile("hosting.json")
        .AddEnvironmentVariables()
        .AddUserSecrets<Program>()
        .Build();
      DumpSettings();

      // Create the container builder.
      var builder = new ContainerBuilder();

      // load all assemblies in same directory and register classes with interfaces
      // Note that we have to explicitly add this (executing) assembly
      var exeAssy = Assembly.GetExecutingAssembly();
      var exeAssyPath = exeAssy.Location;
      var exeAssyDir = Path.GetDirectoryName(exeAssyPath);
      var assyDllPaths = Directory.EnumerateFiles(exeAssyDir, "GraphML.*.dll");
      var assyPaths = new List<string>();
      assyPaths.AddRange(assyDllPaths);

      var assys = assyPaths.Select(filePath => Assembly.LoadFile(filePath)).ToList();
      assys.Add(exeAssy);
      builder
        .RegisterAssemblyTypes(assys.ToArray())
        .PublicOnly()
        .AsImplementedInterfaces()
        .SingleInstance();

      builder.Register(cc => Configuration).As<IConfiguration>();

      // FIX ME!  workaround for Autofac not resolving object in a separate assembly
      builder.RegisterType<DegreeJob>().As<IDegreeJob>();

      // Create the IServiceProvider based on the container.
      var container = builder.Build();
      ServiceProvider = new AutofacServiceProvider(container);

      while (true)
      {
        if (Settings.MESSAGE_QUEUE_USE_THREADS(Configuration))
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
      var msg = RetrieveMessage();
      if (msg != null)
      {
        ProcessMessage(msg);
      }
    }

    private ITextMessage RetrieveMessage()
    {
      var connecturi = new Uri(Settings.MESSAGE_QUEUE_URL(Configuration));
      var factory = new ConnectionFactory(connecturi);
      using (var connection = factory.CreateConnection())
      {
        using (var session = connection.CreateSession())
        {
          using (var destination = SessionUtil.GetQueue(session, Settings.MESSAGE_QUEUE_NAME(Configuration)))
          {
            using (var consumer = session.CreateConsumer(destination))
            {
              connection.Start();
              return consumer.Receive(TimeSpan.FromSeconds(Settings.MESSAGE_QUEUE_POLL_INTERVAL_S(Configuration))) as ITextMessage;
            }
          }
        }
      }
    }

    private void ProcessMessage(ITextMessage msg)
    {
      Console.WriteLine("Message: ");
      Console.WriteLine("  Correlation ID   : " + msg.NMSCorrelationID);
      Console.WriteLine("  Text             : " + msg.Text);
      Console.WriteLine("  NMSTimestamp     : " + msg.NMSTimestamp);

      var jobj = JObject.Parse(msg.Text);
      var typeStr = jobj["Type"].ToString();
      var type = Type.GetType(typeStr);
      var req = JsonConvert.DeserializeObject(msg.Text, type);
      var baseReq = (RequestBase)req;
      var jobType = Type.GetType(baseReq.JobType);
      var job = (IJob)ServiceProvider.GetService(jobType);

      job.Run(baseReq);

      // simulates a log running process
      Thread.Sleep(TimeSpan.FromMinutes(10));
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

    private void DumpSettings()
    {
      Console.WriteLine("Settings:");
      Console.WriteLine($"  MESSAGE_QUEUE:");
      Console.WriteLine($"    MESSAGE_QUEUE_URL               : {Settings.MESSAGE_QUEUE_URL(Configuration)}");
      Console.WriteLine($"    MESSAGE_QUEUE_NAME              : {Settings.MESSAGE_QUEUE_NAME(Configuration)}");
      Console.WriteLine($"    MESSAGE_QUEUE_POLL_INTERVAL_S   : {Settings.MESSAGE_QUEUE_POLL_INTERVAL_S(Configuration)}");
      Console.WriteLine($"    MESSAGE_QUEUE_USE_THREADS       : {Settings.MESSAGE_QUEUE_USE_THREADS(Configuration)}");

      Console.WriteLine($"  LOG:");
      Console.WriteLine($"    LOG_CONNECTION_STRING : {Settings.LOG_CONNECTION_STRING(Configuration)}");
      Console.WriteLine($"    LOG_BEARER_AUTH       : {Settings.LOG_BEARER_AUTH(Configuration)}");

      Console.WriteLine($"  CACHE:");
      Console.WriteLine($"    CACHE_HOST : {Settings.CACHE_HOST(Configuration)}");
    }
  }
}

using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.Util;
using GraphML.Analysis.SNA.Centrality;
using GraphML.Common;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading;

namespace GraphML.Analysis.Server
{
  public sealed class Program
  {
    public IConfiguration Configuration { get; private set; }

    public static void Main(string[] args)
    {
      new Program().Run(args);
    }

    private void Run(string[] args)
    {
      Configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile("hosting.json")
        .AddEnvironmentVariables()
        .AddUserSecrets<Program>()
        .Build();
      DumpSettings();

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
      Console.WriteLine("  Message ID       : " + msg.NMSMessageId);
      Console.WriteLine("  Text             : " + msg.Text);
      Console.WriteLine("  NMSTimestamp     : " + msg.NMSTimestamp);

      var jobj = JObject.Parse(msg.Text);
      var typeStr = jobj["Type"].ToString();
      var type = Type.GetType(typeStr);
      var req = JsonConvert.DeserializeObject(msg.Text, type);
      var baseReq = (BaseRequest)req;

      baseReq.Run();

      // simulates a log running process
      Thread.Sleep(TimeSpan.FromMinutes(10));
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

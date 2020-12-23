using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace GraphML.Common
{
  public static class Settings
  {
    // used by GraphML.API.Server to retrieve data
    public static string API_URI(this IConfiguration config) =>
      Environment.GetEnvironmentVariable("API_URI") ??
      config["API:Uri"];
    public static string API_USERNAME(this IConfiguration config) =>
      Environment.GetEnvironmentVariable("API_USERNAME") ??
      config["API:UserName"];
    public static string API_PASSWORD(this IConfiguration config) =>
      Environment.GetEnvironmentVariable("API_PASSWORD") ??
      config["API:Password"];

    public static string LOG_CONNECTION_STRING(this IConfiguration config) =>
      (Environment.GetEnvironmentVariable("LOG_CONNECTION_STRING") ??
      config["Log:ConnectionString"])?
        .Replace("|DataDirectory|", AppDomain.CurrentDomain.BaseDirectory);

    public static string DATASTORE_CONNECTION(this IConfiguration config) =>
      Environment.GetEnvironmentVariable("DATASTORE_CONNECTION") ??
      config["Datastore:Connection"] ??
      "SqLite";
    public static string DATASTORE_CONNECTION_TYPE(this IConfiguration config, string connection) =>
      Environment.GetEnvironmentVariable("DATASTORE_CONNECTION_TYPE") ??
      config[$"Datastore:{connection}:Type"] ??
      "SqLite";
    public static string DATASTORE_CONNECTION_STRING(this IConfiguration config, string connection) =>
      (Environment.GetEnvironmentVariable("DATASTORE_CONNECTION_STRING") ??
      config[$"Datastore:{connection}:ConnectionString"] ??
      "Data Source=|DataDirectory|Data/GraphML.sqlite3;")?
        .Replace("|DataDirectory|", AppDomain.CurrentDomain.BaseDirectory)
        .Replace('\\', Path.DirectorySeparatorChar)
        .Replace('/', Path.DirectorySeparatorChar)
        .Replace(Path.DirectorySeparatorChar.ToString() + Path.DirectorySeparatorChar.ToString(), Path.DirectorySeparatorChar.ToString());

    public static string CACHE_HOST(this IConfiguration config) =>
      Environment.GetEnvironmentVariable("CACHE_HOST") ??
      config["Cache:Host"] ??
      "localhost";

    public static string RESULT_DATASTORE(this IConfiguration config) =>
      Environment.GetEnvironmentVariable("RESULT_DATASTORE") ??
      config["Result:Datastore"] ??
      "localhost:6379";

    public static string KESTREL_URL(this IConfiguration config) =>
      Environment.GetEnvironmentVariable("KESTREL_URL") ??
      config["Kestrel:EndPoints:Http:Url"] ??
      "http://localhost:5000";

    public static string MESSAGE_QUEUE_URL(this IConfiguration config) =>
      Environment.GetEnvironmentVariable("MESSAGE_QUEUE_URL") ??
      config["Message_Queue:URL"] ??
      "activemq:tcp://localhost:61616";
    public static string MESSAGE_QUEUE_NAME(this IConfiguration config) =>
      Environment.GetEnvironmentVariable("MESSAGE_QUEUE_NAME") ??
      config["Message_Queue:Name"] ??
      "GraphML";
    public static int MESSAGE_QUEUE_POLL_INTERVAL_S(this IConfiguration config) =>
      int.TryParse(
        Environment.GetEnvironmentVariable("MESSAGE_QUEUE_POLL_INTERVAL_S") ??
        config["Message_Queue:Poll_Interval_s"], out int result) ? result : 5;
    public static bool MESSAGE_QUEUE_USE_THREADS(this IConfiguration config) =>
      bool.Parse(
        Environment.GetEnvironmentVariable("MESSAGE_QUEUE_USE_THREADS") ??
        config["Message_Queue:Use_Threads"] ??
        false.ToString());

    public static string IDENTITY_SERVER_BASE_URL(this IConfiguration config) =>
        Environment.GetEnvironmentVariable("IDENTITY_SERVER_BASE_URL") ??
        config["Identity_Server:Base_Url"] ??
        false.ToString();


    public static void DumpSettings(IConfiguration config)
    {
      var root = (IConfigurationRoot)config;
      var debugView = root.GetDebugView();
      Console.WriteLine(debugView);
      Console.WriteLine();

      Console.WriteLine("Settings:");
      Console.WriteLine($"  IDENTITY_SERVER:");
      Console.WriteLine($"    BASE_URL      : {config.IDENTITY_SERVER_BASE_URL()}");

      Console.WriteLine($"  API:");
      Console.WriteLine($"    API_URI       : {config.API_URI()}");
      Console.WriteLine($"    API_USERNAME  : {config.API_USERNAME()}");
      Console.WriteLine($"    API_PASSWORD  : {config.API_PASSWORD()}");

      Console.WriteLine($"  DATASTORE:");
      Console.WriteLine($"    DATASTORE_CONNECTION         : {config.DATASTORE_CONNECTION()}");
      Console.WriteLine($"    DATASTORE_CONNECTION_TYPE    : {config.DATASTORE_CONNECTION_TYPE(config.DATASTORE_CONNECTION())}");
      Console.WriteLine($"    DATASTORE_CONNECTION_STRING  : {config.DATASTORE_CONNECTION_STRING(config.DATASTORE_CONNECTION())}");

      Console.WriteLine($"  LOG:");
      Console.WriteLine($"    LOG_CONNECTION_STRING        : {config.LOG_CONNECTION_STRING()}");

      Console.WriteLine($"  RESULT:");
      Console.WriteLine($"    RESULT_DATASTORE  : {config.RESULT_DATASTORE()}");

      Console.WriteLine($"  KESTREL:");
      Console.WriteLine($"    KESTREL_URL       : {config.KESTREL_URL()}");

      Console.WriteLine($"  MESSAGE_QUEUE:");
      Console.WriteLine($"    MESSAGE_QUEUE_URL               : {config.MESSAGE_QUEUE_URL()}");
      Console.WriteLine($"    MESSAGE_QUEUE_NAME              : {config.MESSAGE_QUEUE_NAME()}");
      Console.WriteLine($"    MESSAGE_QUEUE_POLL_INTERVAL_S   : {config.MESSAGE_QUEUE_POLL_INTERVAL_S()}");
      Console.WriteLine($"    MESSAGE_QUEUE_USE_THREADS       : {config.MESSAGE_QUEUE_USE_THREADS()}");
    }
  }
}

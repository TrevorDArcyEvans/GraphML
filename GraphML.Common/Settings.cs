using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GraphML.Common
{
  public static class Settings
  {
    // used by GraphML.API.Server to retrieve data
    public static string API_URI(this IConfiguration config) =>
      Environment.GetEnvironmentVariable("API_URI") ??
      config["API:Uri"];

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

    public static string RESULT_DATASTORE(this IConfiguration config) =>
      Environment.GetEnvironmentVariable("RESULT_DATASTORE") ??
      config["Result:Datastore"] ??
      "localhost:6379";

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
        config["Message_Queue:Poll_Interval_s"], out int result)
        ? result
        : 5;

    public static bool MESSAGE_QUEUE_USE_THREADS(this IConfiguration config) =>
      bool.Parse(
        Environment.GetEnvironmentVariable("MESSAGE_QUEUE_USE_THREADS") ??
        config["Message_Queue:Use_Threads"] ??
        false.ToString());

    public static string IDENTITY_SERVER_BASE_URL(this IConfiguration config) =>
      Environment.GetEnvironmentVariable("IDENTITY_SERVER_BASE_URL") ??
      config["Identity_Server:Base_Url"] ??
      "https://localhost:44387";

    public static string IDENTITY_SERVER_AUTHORIZATION_REL_URL(this IConfiguration config) =>
      Environment.GetEnvironmentVariable("IDENTITY_SERVER_AUTHORIZATION_REL_URL") ??
      config["Identity_Server:Authorization_Rel_Url"] ??
      "/connect/authorize";

    public static string IDENTITY_SERVER_TOKEN_REL_URL(this IConfiguration config) =>
      Environment.GetEnvironmentVariable("IDENTITY_SERVER_TOKEN_REL_URL") ??
      config["Identity_Server:Token_Rel_Url"] ??
      "/connect/token";

    public static string IDENTITY_SERVER_AUDIENCE(this IConfiguration config) =>
      Environment.GetEnvironmentVariable("IDENTITY_SERVER_AUDIENCE") ??
      config["Identity_Server:Audience"] ??
      "*** no Audience ***";

    public static string IDENTITY_SERVER_CLIENT_ID(this IConfiguration config) =>
      Environment.GetEnvironmentVariable("IDENTITY_SERVER_CLIENT_ID") ??
      config["Identity_Server:Client_Id"] ??
      "*** no Client Id ***";

    public static string IDENTITY_SERVER_CLIENT_SECRET(this IConfiguration config) =>
      Environment.GetEnvironmentVariable("IDENTITY_SERVER_CLIENT_SECRET") ??
      config["Identity_Server:Client_Secret"] ??
      "*** no Client Secret ***";

    public static IEnumerable<string> IDENTITY_SERVER_SCOPES(this IConfiguration config)
    {
      var scopes = config
        .GetSection("Identity_Server:Scopes")
        .GetChildren()
        .Select(child => child.Value);
      return Environment.GetEnvironmentVariable("IDENTITY_SERVER_SCOPES")?.Split(',') ??
             scopes;
    }


    public static void DumpSettings(IConfiguration config)
    {
      var root = (IConfigurationRoot) config;
      var debugView = root.GetDebugView();
      Console.WriteLine(debugView);
      Console.WriteLine();

      Console.WriteLine("Settings:");
      Console.WriteLine($"  IDENTITY_SERVER:");
      Console.WriteLine($"    BASE_URL                      : {config.IDENTITY_SERVER_BASE_URL()}");
      Console.WriteLine($"    AUTHORIZATION_REL_URL         : {config.IDENTITY_SERVER_AUTHORIZATION_REL_URL()}");
      Console.WriteLine($"    TOKEN_REL_URL                 : {config.IDENTITY_SERVER_TOKEN_REL_URL()}");
      Console.WriteLine($"    AUDIENCE                      : {config.IDENTITY_SERVER_AUDIENCE()}");
      Console.WriteLine($"    CLIENT_ID                     : {config.IDENTITY_SERVER_CLIENT_ID()}");
      Console.WriteLine($"    CLIENT_SECRET                 : {config.IDENTITY_SERVER_CLIENT_SECRET()}");
      Console.WriteLine($"    SCOPES                        : {string.Join(','.ToString(), config.IDENTITY_SERVER_SCOPES())}");

      Console.WriteLine($"  API:");
      Console.WriteLine($"    API_URI       : {config.API_URI()}");

      Console.WriteLine($"  DATASTORE:");
      Console.WriteLine($"    DATASTORE_CONNECTION         : {config.DATASTORE_CONNECTION()}");
      Console.WriteLine($"    DATASTORE_CONNECTION_TYPE    : {config.DATASTORE_CONNECTION_TYPE(config.DATASTORE_CONNECTION())}");
      Console.WriteLine($"    DATASTORE_CONNECTION_STRING  : {config.DATASTORE_CONNECTION_STRING(config.DATASTORE_CONNECTION())}");

      Console.WriteLine($"  LOG:");
      Console.WriteLine($"    LOG_CONNECTION_STRING        : {config.LOG_CONNECTION_STRING()}");

      Console.WriteLine($"  RESULT:");
      Console.WriteLine($"    RESULT_DATASTORE  : {config.RESULT_DATASTORE()}");

      Console.WriteLine($"  MESSAGE_QUEUE:");
      Console.WriteLine($"    MESSAGE_QUEUE_URL               : {config.MESSAGE_QUEUE_URL()}");
      Console.WriteLine($"    MESSAGE_QUEUE_NAME              : {config.MESSAGE_QUEUE_NAME()}");
      Console.WriteLine($"    MESSAGE_QUEUE_POLL_INTERVAL_S   : {config.MESSAGE_QUEUE_POLL_INTERVAL_S()}");
      Console.WriteLine($"    MESSAGE_QUEUE_USE_THREADS       : {config.MESSAGE_QUEUE_USE_THREADS()}");
    }
  }
}

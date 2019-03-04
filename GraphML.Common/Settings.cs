using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace GraphML.Common
{
#pragma warning disable CS1591
  public static class Settings
  {
    public static string OIDC_ISSUER_URL(IConfiguration config) => Environment.GetEnvironmentVariable("OIDC_ISSUER_URL") ?? config["OIDC:Issuer_URL"];
    public static string OIDC_AUDIENCE(IConfiguration config) => Environment.GetEnvironmentVariable("OIDC_AUDIENCE") ?? config["OIDC:Audience"];
    public static string OIDC_USERINFO_URL(IConfiguration config) => Environment.GetEnvironmentVariable("OIDC_USERINFO_URL") ?? config["OIDC:UserInfo_URL"];

    public static string LOG_CONNECTION_STRING(IConfiguration config) => (Environment.GetEnvironmentVariable("LOG_CONNECTION_STRING") ?? config["Log:ConnectionString"])?.Replace("|DataDirectory|", AppDomain.CurrentDomain.BaseDirectory);
    public static bool LOG_BEARER_AUTH(IConfiguration config) => bool.Parse(Environment.GetEnvironmentVariable("LOG_BEARER_AUTH") ?? config["Log:Bearer_Auth"] ?? false.ToString());

    public static string DATASTORE_CONNECTION(IConfiguration config) => Environment.GetEnvironmentVariable("DATASTORE_CONNECTION") ?? config["Datastore:Connection"];
    public static string DATASTORE_CONNECTION_TYPE(IConfiguration config, string connection) => Environment.GetEnvironmentVariable("DATASTORE_CONNECTION_TYPE") ?? config[$"Datastore:{connection}:Type"];
    public static string DATASTORE_CONNECTION_STRING(IConfiguration config, string connection) => (Environment.GetEnvironmentVariable("DATASTORE_CONNECTION_STRING") ?? config[$"Datastore:{connection}:ConnectionString"])?
      .Replace("|DataDirectory|", AppDomain.CurrentDomain.BaseDirectory)
      .Replace('\\', Path.DirectorySeparatorChar)
      .Replace('/', Path.DirectorySeparatorChar)
      .Replace(Path.DirectorySeparatorChar.ToString() + Path.DirectorySeparatorChar.ToString(), Path.DirectorySeparatorChar.ToString());

    public static string RESULT_DATASTORE(IConfiguration config) => Environment.GetEnvironmentVariable("RESULT_DATASTORE") ?? config["Result:Datastore"] ?? "localhost";

    public static string KESTREL_CERTIFICATE_FILENAME(IConfiguration config) => Environment.GetEnvironmentVariable("KESTREL_CERTIFICATE_FILENAME") ?? config["Kestrel:Certificate_FileName"] ?? "GraphML.pfx";
    public static string KESTREL_CERTIFICATE_PASSWORD(IConfiguration config) => Environment.GetEnvironmentVariable("KESTREL_CERTIFICATE_PASSWORD") ?? config["Kestrel:Certificate_Password"] ?? "DisruptTheMarket";
    public static string KESTREL_URLS(IConfiguration config) => Environment.GetEnvironmentVariable("KESTREL_URLS") ?? config["Kestrel:Urls"] ?? "http://localhost:5000";
    public static int KESTREL_HTTPS_PORT(IConfiguration config) => int.TryParse(Environment.GetEnvironmentVariable("KESTREL_HTTPS_PORT") ?? config["Kestrel:Https_Port"], out int result) ? result : 8000;

    public static string MESSAGE_QUEUE_URL(IConfiguration config) => Environment.GetEnvironmentVariable("MESSAGE_QUEUE_URL") ?? config["Message_Queue:URL"] ?? "activemq:tcp://localhost:61616";
    public static string MESSAGE_QUEUE_NAME(IConfiguration config) => Environment.GetEnvironmentVariable("MESSAGE_QUEUE_NAME") ?? config["Message_Queue:Name"] ?? "GraphML";
    public static int MESSAGE_QUEUE_POLL_INTERVAL_S(IConfiguration config) => int.TryParse(Environment.GetEnvironmentVariable("MESSAGE_QUEUE_POLL_INTERVAL_S") ?? config["Message_Queue:Poll_Interval_s"], out int result) ? result : 5;
    public static bool MESSAGE_QUEUE_USE_THREADS(IConfiguration config) => bool.Parse(Environment.GetEnvironmentVariable("MESSAGE_QUEUE_USE_THREADS") ?? config["Message_Queue:Use_Threads"] ?? false.ToString());

    public static void DumpSettings(IConfiguration config)
    {
      Console.WriteLine("Settings:");
      Console.WriteLine($"  DATASTORE:");
      Console.WriteLine($"    DATASTORE_CONNECTION         : {Settings.DATASTORE_CONNECTION(config)}");
      Console.WriteLine($"    DATASTORE_CONNECTION_TYPE    : {Settings.DATASTORE_CONNECTION_TYPE(config, Settings.DATASTORE_CONNECTION(config))}");
      Console.WriteLine($"    DATASTORE_CONNECTION_STRING  : {Settings.DATASTORE_CONNECTION_STRING(config, Settings.DATASTORE_CONNECTION(config))}");

      Console.WriteLine($"  LOG:");
      Console.WriteLine($"    LOG_CONNECTION_STRING : {Settings.LOG_CONNECTION_STRING(config)}");
      Console.WriteLine($"    LOG_BEARER_AUTH       : {Settings.LOG_BEARER_AUTH(config)}");

      Console.WriteLine($"  OIDC:");
      Console.WriteLine($"    OIDC_USERINFO_URL : {Settings.OIDC_USERINFO_URL(config)}");
      Console.WriteLine($"    OIDC_ISSUER_URL   : {Settings.OIDC_ISSUER_URL(config)}");
      Console.WriteLine($"    OIDC_AUDIENCE     : {Settings.OIDC_AUDIENCE(config)}");

      Console.WriteLine($"  CACHE:");
      Console.WriteLine($"    CACHE_HOST : {Settings.RESULT_DATASTORE(config)}");

      Console.WriteLine($"  KESTREL:");
      Console.WriteLine($"    KESTREL_CERTIFICATE_FILENAME  : {Settings.KESTREL_CERTIFICATE_FILENAME(config)}");
      Console.WriteLine($"    KESTREL_CERTIFICATE_PASSWORD  : {Settings.KESTREL_CERTIFICATE_PASSWORD(config)}");
      Console.WriteLine($"    KESTREL_URLS                  : {Settings.KESTREL_URLS(config)}");
      Console.WriteLine($"    KESTREL_HTTPS_PORT            : {Settings.KESTREL_HTTPS_PORT(config)}");

      Console.WriteLine($"  MESSAGE_QUEUE:");
      Console.WriteLine($"    MESSAGE_QUEUE_URL               : {Settings.MESSAGE_QUEUE_URL(config)}");
      Console.WriteLine($"    MESSAGE_QUEUE_NAME              : {Settings.MESSAGE_QUEUE_NAME(config)}");
      Console.WriteLine($"    MESSAGE_QUEUE_POLL_INTERVAL_S   : {Settings.MESSAGE_QUEUE_POLL_INTERVAL_S(config)}");
      Console.WriteLine($"    MESSAGE_QUEUE_USE_THREADS       : {Settings.MESSAGE_QUEUE_USE_THREADS(config)}");
    }


  }
#pragma warning restore CS1591
}

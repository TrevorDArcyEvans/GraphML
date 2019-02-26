using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace GraphML.Utils
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

    public static string CACHE_HOST(IConfiguration config) => Environment.GetEnvironmentVariable("CACHE_HOST") ?? config["Cache:Host"] ?? "localhost";

    public static string KESTREL_CERTIFICATE_FILENAME(IConfiguration config) => Environment.GetEnvironmentVariable("KESTREL_CERTIFICATE_FILENAME") ?? config["Kestrel:Certificate_FileName"] ?? "GraphML.pfx";
    public static string KESTREL_CERTIFICATE_PASSWORD(IConfiguration config) => Environment.GetEnvironmentVariable("KESTREL_CERTIFICATE_PASSWORD") ?? config["Kestrel:Certificate_Password"] ?? "DisruptTheMarket";
    public static string KESTREL_URLS(IConfiguration config) => Environment.GetEnvironmentVariable("KESTREL_URLS") ?? config["Kestrel:Urls"] ?? "http://localhost:5000";
    public static int KESTREL_HTTPS_PORT(IConfiguration config) => int.TryParse(Environment.GetEnvironmentVariable("KESTREL_HTTPS_PORT") ?? config["Kestrel:Https_Port"], out int result) ? result : 8000;
  }
#pragma warning restore CS1591
}

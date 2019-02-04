using Microsoft.Extensions.Configuration;
using System;

namespace GraphML.Utils
{
#pragma warning disable CS1591
  public static class Settings
  {
    public static string OIDC_ISSUER_URL(IConfiguration config) => Environment.GetEnvironmentVariable("OIDC_ISSUER_URL") ?? config["OIDC:Issuer_URL"];
    public static string OIDC_AUDIENCE(IConfiguration config) => Environment.GetEnvironmentVariable("OIDC_AUDIENCE") ?? config["OIDC:Audience"];
    public static string OIDC_USERINFO_URL(IConfiguration config) => Environment.GetEnvironmentVariable("OIDC_USERINFO_URL") ?? config["OIDC:UserInfo_URL"];

    public static string LOG_CONNECTION_STRING(IConfiguration config) => Environment.GetEnvironmentVariable("LOG_CONNECTIONSTRING") ?? config["Log:Connection_String"];
    public static bool LOG_BEARER_AUTH(IConfiguration config) => bool.Parse(Environment.GetEnvironmentVariable("LOG_BEARERAUTH") ?? config["Log:Bearer_Auth"] ?? false.ToString());

    public static string DATASTORE_CONNECTION(IConfiguration config) => Environment.GetEnvironmentVariable("DATASTORE_CONNECTION") ?? config["Datastore:Connection"];
    public static string DATASTORE_CONNECTION_TYPE(IConfiguration config, string connection) => Environment.GetEnvironmentVariable("DATASTORE_CONNECTION_TYPE") ?? config[$"Datastore:{connection}:Type"];
    public static string DATASTORE_CONNECTION_STRING(IConfiguration config, string connection) => (Environment.GetEnvironmentVariable("DATASTORE_CONNECTION_STRING") ?? config[$"Datastore:{connection}:ConnectionString"]);

    public static string CACHE_HOST(IConfiguration config) => Environment.GetEnvironmentVariable("CACHE_HOST") ?? config["Cache:Host"] ?? "localhost";
  }
#pragma warning restore CS1591
}

using Microsoft.Extensions.Configuration;
using System;

namespace GraphML.API.Server
{
  public static class Settings
  {
    public static string LOG_CONNECTIONSTRING(IConfiguration config) => Environment.GetEnvironmentVariable("LOG_CONNECTIONSTRING") ?? config["Log:ConnectionString"];

    public static string DATASTORE_CONNECTION(IConfiguration config) => Environment.GetEnvironmentVariable("DATASTORE_CONNECTION") ?? config["RepositoryDatabase:Connection"];
    public static string DATASTORE_CONNECTIONTYPE(IConfiguration config, string connection) => Environment.GetEnvironmentVariable("DATASTORE_CONNECTIONTYPE") ?? config[$"RepositoryDatabase:{connection}:Type"];
    public static string DATASTORE_CONNECTIONSTRING(IConfiguration config, string connection) => (Environment.GetEnvironmentVariable("DATASTORE_CONNECTIONSTRING") ?? config[$"RepositoryDatabase:{connection}:ConnectionString"]);

    public static string API_URI(IConfiguration config) => Environment.GetEnvironmentVariable("API_URI") ?? config["API:Uri"];
    public static string API_USERNAME(IConfiguration config) => Environment.GetEnvironmentVariable("API_USERNAME") ?? config["API:UserName"];
    public static string API_PASSWORD(IConfiguration config) => Environment.GetEnvironmentVariable("API_PASSWORD") ?? config["API:Password"];

    public static string CACHE_HOST(IConfiguration config) => Environment.GetEnvironmentVariable("CACHE_HOST") ?? config["Cache:Host"] ?? "localhost";
  }
}

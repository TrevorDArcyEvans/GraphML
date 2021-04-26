using System;
using System.Data;
using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;
using Polly;

namespace GraphML.Datastore.Database
{
  public abstract class DatastoreBase
  {
    protected readonly IDbConnection _dbConnection;
    private readonly ISyncPolicy _policy;

    public DatastoreBase(
      IDbConnectionFactory dbConnectionFactory,
      ILogger logger,
      ISyncPolicyFactory policy)
    {
      _dbConnection = dbConnectionFactory.Get();
      _policy = policy.Build(logger);
    }

    protected string AppendForFetch(int pageIndex, int pageSize)
    {
      var dbType = _dbConnection.GetType().ToString();
      switch (dbType)
      {
        case "System.Data.SQLite.SQLiteConnection":
          return $"{Environment.NewLine} limit {pageSize} offset {pageIndex * pageSize}";

        case "MySql.Data.MySqlClient.MySqlConnection":
          return $"{Environment.NewLine} limit {pageIndex * pageSize}, {pageSize}";

        case "Npgsql.NpgsqlConnection":
          return $"{Environment.NewLine} limit {pageSize} offset {pageIndex * pageSize}";

        case "System.Data.SqlClient.SqlConnection":
          return $"{Environment.NewLine} offset {pageIndex * pageSize} rows fetch next {pageSize} rows only";

        default:
          throw new ArgumentOutOfRangeException($"Untested database: {dbType}");
      }
    }

    protected TOther GetInternal<TOther>(Func<TOther> get)
    {
      return _policy.Execute(get);
    }
  }
}

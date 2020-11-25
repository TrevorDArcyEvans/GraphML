using Dapper.Contrib.Extensions;
using GraphML.Datastore.Database.Interfaces;
using GraphML.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Linq;
using System.Reflection;

namespace GraphML.Datastore.Database
{
  public sealed class DbConnectionFactory : IDbConnectionFactory
  {
    private readonly IConfiguration _config;

    public DbConnectionFactory(IConfiguration config)
    {
      _config = config;
    }

    public IDbConnection Get()
    {
      var connection = _config.DATASTORE_CONNECTION();
      var connType = _config.DATASTORE_CONNECTION_TYPE(connection);
      var dbType = Enum.Parse<DataAccessProviderTypes>(connType);

      // HACK:  workaround for PostgreSql which puts table+column names in lower case
      if (dbType == DataAccessProviderTypes.PostgreSql)
      {
        SqlMapperExtensions.TableNameMapper = LowerCaseTableNameMapper;
      }

      var dbFact = DbProviderFactoryUtils.GetDbProviderFactory(dbType);
      var dbConn = dbFact.CreateConnection();

      dbConn.ConnectionString = _config.DATASTORE_CONNECTION_STRING(connection);
      dbConn.Open();

      return dbConn;
    }

    private static string LowerCaseTableNameMapper(Type type)
    {
      var tableattr = type.GetCustomAttributes<TableAttribute>(false).SingleOrDefault();
      var name = string.Empty;

      if (tableattr != null)
      {
        name = tableattr.Name.ToLowerInvariant();
      }

      return name;
    }
  }
}

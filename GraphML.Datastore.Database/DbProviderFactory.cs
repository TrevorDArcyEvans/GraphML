using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using Westwind.Utilities;
using Westwind.Utilities.Properties;

namespace GraphML.Datastore.Database
{
  // stolen from:
  //
  //    https://weblog.west-wind.com/posts/2017/Nov/27/Working-around-the-lack-of-dynamic-DbProviderFactory-loading-in-NET-Core

  internal enum DataAccessProviderTypes
  {
    SqlServer,
    SqLite,
    MySql,
    PostgreSql
  }

  internal static class DbProviderFactoryUtils
  {
    public static DbProviderFactory GetDbProviderFactory(DataAccessProviderTypes type)
    {
      if (type == DataAccessProviderTypes.SqlServer)
      {
        return SqlClientFactory.Instance; // this library has a ref to SqlClient so this works
      }

      if (type == DataAccessProviderTypes.SqLite)
      {
        return SQLiteFactory.Instance;
      }

      if (type == DataAccessProviderTypes.MySql)
      {
        return GetDbProviderFactory("MySql.Data.MySqlClient.MySqlClientFactory", "MySql.Data");
      }

      if (type == DataAccessProviderTypes.PostgreSql)
      {
        return GetDbProviderFactory("Npgsql.NpgsqlFactory", "Npgsql");
      }

      throw new NotSupportedException(string.Format(Resources.UnsupportedProviderFactory, type.ToString()));
    }

    private static DbProviderFactory GetDbProviderFactory(string dbProviderFactoryTypename, string assemblyName)
    {
      var instance = ReflectionUtils.GetStaticProperty(dbProviderFactoryTypename, "Instance");
      if (instance == null)
      {
        var a = ReflectionUtils.LoadAssembly(assemblyName);
        if (a != null)
        {
          instance = ReflectionUtils.GetStaticProperty(dbProviderFactoryTypename, "Instance");
        }
      }

      if (instance == null)
      {
        throw new InvalidOperationException(string.Format(Resources.UnableToRetrieveDbProviderFactoryForm, dbProviderFactoryTypename));
      }

      return instance as DbProviderFactory;
    }
  }
}

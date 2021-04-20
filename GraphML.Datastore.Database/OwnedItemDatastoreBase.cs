﻿using Dapper;
using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace GraphML.Datastore.Database
{
  public abstract class OwnedItemDatastoreBase<T> : DatastoreBase<T>, IOwnedDatastore<T> where T : OwnedItem
  {
    public OwnedItemDatastoreBase(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<OwnedItemDatastoreBase<T>> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }

    public virtual IEnumerable<T> ByOwners(IEnumerable<Guid> ownerIds, int pageIndex, int pageSize)
    {
      return GetInternal(() =>
      {
        // TODO   PageableDataEx
        var sql = $"select * from {GetTableName()} where {nameof(OwnedItem.OwnerId)} in ({GetListIds(ownerIds)}) order by {nameof(OwnedItem.Name)} {AppendForFetch(pageIndex, pageSize)}";

        return _dbConnection.Query<T>(sql);
      });
    }

    public int Count(Guid ownerId)
    {
      return GetInternal(() =>
      {
        var sql = $"select count(*) from {GetTableName()} where {nameof(OwnedItem.OwnerId)} = '{ownerId}'";

        return _dbConnection.QueryFirst<int>(sql);
      });
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
  }
}

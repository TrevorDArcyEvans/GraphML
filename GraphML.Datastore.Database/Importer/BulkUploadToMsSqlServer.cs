using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace GraphML.Datastore.Database.Importer.CSV
{
  public sealed class BulkUploadToMsSqlServer
  {
    public int CommitBatchSize { get; set; } = 1000;

    private readonly SqlConnection _conn;
    private readonly SqlTransaction _trans;

    public BulkUploadToMsSqlServer(SqlConnection conn, SqlTransaction trans)
    {
      _conn = conn;
      _trans = trans;
    }

    public void Commit<T>(IList<T> data, string tableName)
    {
      if (data.Count > 0)
      {
        DataTable dt;
        int numberOfPages = (data.Count / CommitBatchSize) + (data.Count % CommitBatchSize == 0 ? 0 : 1);
        for (int pageIndex = 0; pageIndex < numberOfPages; pageIndex++)
        {
          dt = data.Skip(pageIndex * CommitBatchSize).Take(CommitBatchSize).ToDataTable();
          dt.TableName = tableName;
          BulkInsert(dt);
        }
      }
    }

    private void BulkInsert(DataTable dt)
    {
      // make sure to enable triggers
      // more on triggers in next post
      SqlBulkCopy bulkCopy = new SqlBulkCopy(
        _conn,
        SqlBulkCopyOptions.TableLock |
        SqlBulkCopyOptions.FireTriggers,
        _trans);

      // set the destination table name
      bulkCopy.DestinationTableName = dt.TableName;

      // ADD COLUMN MAPPING
      foreach (DataColumn col in dt.Columns)
      {
        bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
      }

      // write the data in the "dataTable"
      bulkCopy.WriteToServer(dt);
    }
  }
}

# GraphML Database Importer
* import csv or tsv data
* super fast import with _Microsoft SQL Server_!

## Details
* each data row is an edge between two nodes
* column[0] --> Source
* column[1] --> Target

## Import Specification

```json
{
  "Datastore": {
    "Connection": "SqLite",

    "SqLite": {
      "Type": "SqLite",
      "ConnectionString": "Data Source=|DataDirectory|Data/GraphML.sqlite3;"
    },

    "SqlServer": {
      "Type": "SqlServer",
      "ConnectionString": "Data Source=localhost;Initial Catalog=GraphML;Integrated Security=True;MultipleActiveResultSets=True;"
    },

    "MySql": {
      "Type": "MySql",
      "ConnectionString": "server=127.0.0.1;uid=GraphML;pwd=DisruptTheMarket;database=GraphML;SslMode=none"
    }
  },
  "ImportSpecification": {
    "Organisation": "GraphML",
    "RepositoryManager": "GraphML Repository Manager",
    "Repository": "Reddit",

    "DataFile": "../../../Sample-Data/SNAP/soc-redditHyperlinks-body-small.tsv",
    "HasHeaderRecord": true
  }
}
```
| Section             | Field | Description | Notes |
|---------------------|-------|-------------|-------|
| Datastore           | Connection | name of database configuration |  |
|                     | [_Connection_]:Type | type of database | `SqLite\SqlServer\MySql\PostgreSql` |
|                     | [_Connection_]:ConnectionString | _.NET_ database connection string | examples at [ConnectonStrings.com](https://www.connectionstrings.com/) |
| ImportSpecification | Organisation | name of _Organisation_ | must exist |
|                     | RepositoryManager | name of _Repository Manager_ | must exist |
|                     | Repository | name of _Repository_ | will be created if it does not exist |
|                     | DataFile | path to csv or tsv data file |  |
|                     | HasHeaderRecord | if first line of _DataFile_ is a header | `true/false` |

## Further Work
* attributes
  * string
  * bool
  * int
  * double
  * date-time
  * duration
* edge weight (via attribute)
* edge name


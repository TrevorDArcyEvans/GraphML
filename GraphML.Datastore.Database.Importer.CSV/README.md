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
    "Repository": "Bitcoin-Alpha",

    "DataFile": "../../../Sample-Data/SNAP/soc-sign-bitcoinalpha.csv",
    "HasHeaderRecord": false,
    "NodeItemAttributeDefinition" : [],
    "EdgeItemAttributeDefinitions": [
      {
        "Name": "Rating",
        "DataType": "int",
        "Column": 2
      },
      {
        "Name": "Time",
        "DataType": "DateTime",
        "Column": 3
      }
    ]
  }
}
```
**Datastore**

| Field | Description | Notes |
|-------|-------------|-------|
| Connection | name of database configuration |  |
| [_Connection_]:Type | type of database | `SqLite\SqlServer\MySql\PostgreSql` |
| [_Connection_]:ConnectionString | _.NET_ database connection string | examples at [ConnectonStrings.com](https://www.connectionstrings.com/) |

**ImportSpecification**

| Field | Description | Notes |
|-------|-------------|-------|
| Organisation | name of _Organisation_ | must exist |
| RepositoryManager | name of _Repository Manager_ | must exist |
| Repository | name of _Repository_ | will be created if it does not exist |
| DataFile | path to csv or tsv data file |  |
| HasHeaderRecord | if first line of _DataFile_ is a header | `true/false` |
| NodeItemAttributeDefinition | collection of attributes to apply to each node | will be created if it does not exist<p/>if does exist, must be same data type |
| EdgeItemAttributeDefinitions | collection of attributes to apply to each edge | will be created if it does not exist<p/>if does exist, must be same data type |

### Supported Attribute Data Types
* string
* bool
* int
* double
* DateTime

Default.NET parser is used to convert from a string using invariant culture

## Further Work
* attribute data types
    * DateInterval
      * [Noda Time](https://nodatime.org/2.0.x/api/NodaTime.DateInterval.html)
      * [ST-Software/Utils](https://github.com/ST-Software/Utils/blob/master/src/DateTimeUtils.cs)
* edge weight (via attribute)
* edge name


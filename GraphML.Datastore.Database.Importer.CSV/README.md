# GraphML Database Importer
* import csv or tsv data
* super fast import with _Microsoft SQL Server_!
* each data row is an edge between two nodes
  * or a single node
* add attributes to nodes and edges
* icons for source and target nodes
* automatically create a default _Graph_ and _Chart_

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

    "NodeItemAttributeImportDefinitions": [
      {
        "Name": "Depth",
        "DataType": "int",
        "ApplyTo": "SourceNode",
        "Columns": [
          2
        ]
      }
    ],
    "EdgeItemAttributeImportDefinitions": [
      {
        "Name": "Rating",
        "DataType": "int",
        "Columns": [
          2
        ]
      },
      {
        "Name": "Time",
        "DataType": "DateTime",
        "DateTimeFormat": "SecondsSinceUnixEpoch",
        "Columns": [
          3
        ]
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
| SourceNodeColumn | zero based column index for SourceNode name/identifier | defaults to 0 |
| TargetNodeColumn | zero based column index for TargetNode name/identifier | defaults to 1 |
| SourceIconName | relative path of image file (including extension) from 'wwwroot' directory when rendering an Icon on a Diagram surface | defaults to `null` ie no icon |
| TargetIconName | relative path of image file (including extension) from 'wwwroot' directory when rendering an Icon on a Diagram surface | defaults to `null` ie no icon |
| NodeItemAttributeImportDefinitions | collection of attributes to apply to each node | will be created if it does not exist<p/>if does exist, must be same data type |
| EdgeItemAttributeImportDefinitions | collection of attributes to apply to each edge | will be created if it does not exist<p/>if does exist, must be same data type |

**ItemAttributeImportDefinition**

| Field   | Description | Notes |
|---------|-------------|-------|
| Columns | zero based array column index for attribute value | for _DateTimeInterval_:<p/>first index is _Start_<p/>second index is _End_ |

**NodeItemAttributeImportDefinition**

Note that a node attribute may be applied multiple times.

| Field | Description | Notes |
|-------|-------------|-------|
| ApplyTo | to which node/s to add attribute | defaults to `SourceNode` |

### Supported Attribute Data Types
* string
* bool
* int
* double
* DateTime
  * assumed to be in UTC
  * stored in [ISO 8601](http://en.wikipedia.org/wiki/ISO_8601) format
* DateTimeInterval
  
### Supported Attribute DateTimeFormat
* all [.NET Standard date and time format strings](https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings) using invariant culture
* SecondsSinceUnixEpoch
  * number of seconds since January 1, 1970, at 12:00 AM UTC
* if not specified, will try generic parse using invariant culture

### ApplyTo
* SourceNode
* TargetNode
* BothNodes

## Further Work
* edge name
  * currently $"{SourceNode.Name}-->{TargetNode.Name}"

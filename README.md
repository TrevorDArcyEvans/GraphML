# GraphML ![](Readme-Docs/GraphML.icon.png)
GraphML analyses graphs for the following measures:

* <details>
    <summary>ranked shortest paths</summary>
These calculations help your users understand ways to travel through (or ‘traverse’) a network.<p/>
The distance function measures how many hops apart two nodes are in a network. Shortest path 
highlights the route that passes through the lowest number of nodes. <p/>
Hops can also be weighted, meaning you can calculate actual distances, as well as the number of hops.<p/>

  [Wikipedia](https://en.wikipedia.org/wiki/K_shortest_path_routing)
  </details>
* Social Network Analysis (SNA)
  * <details>
      <summary>closeness</summary>
This is the measure that helps you find the nodes that are closest to the other nodes in a network,
based on their ability to reach them.<p/>
To calculate this, the algorithm finds the shortest path between each node, then assigns each node a 
score based on the sum of all the paths.<p/>
Nodes with a high closeness value have a lower distance to all other nodes. They’d be efficient broadcasters of information.</p> 

      [Wikipedia](https://en.wikipedia.org/wiki/Centrality)
    </details>
  * <details>
      <summary>betweeness</summary>
Nodes with a high betweenness centrality score are the ones that most frequently act as ‘bridges’ between other nodes. 
They form the shortest pathways of communication within the network.<p/>
Usually this would indicate important gatekeepers of information between groups.<p/>

    [Wikipedia](https://en.wikipedia.org/wiki/Betweenness_centrality) <p/>
    </details>
  * <details>
      <summary>degree</summary>
The degree centrality measure finds nodes with the highest number of links to other nodes in the network.<p/>
Nodes with a high degree centrality have the best connections to those around them – they might be influential,
or just strategically well-placed.<p/>

    [Wikipedia]( https://en.wikipedia.org/wiki/Degree_(graph_theory) ) <p/>
    </details>

## Prerequisites
<details>
</p>

1. Prerequisites:
  * host:
    * Linux
    * Windows (not tested but should work)
  * target:
    * Linux (services)
    * WebAssembly aka WASM (GUI)
  * .NET Core SDK v5.0
  * integrated development environment:
    * Visual Studio Code (Linux or Windows)
    * JetBrains Rider (Linux or Windows)
    * Visual Studio (Windows)
  * nodejs
  * git
  * Google Chrome web browser
  * database:
    * Microsoft SQL Server
    * MySQL or MariaDB
    * PostgreSQL
    * SQLite (local development only)
  * message queue:
    * [Apache ActiveMQ](http://activemq.apache.org/)
  * results store:
    * [Redis](https://redis.io/)
1. Optional
  * [Git Extensions](https://github.com/gitextensions/gitextensions) (Windows)
  * [Docker](https://docs.docker.com/docker-for-windows/install/) (Windows)
  * [SwitchStartupProject for VS 2019](https://heptapod.host/thirteen/switchstartupproject/) (Visual Studio)
  * [npm](https://www.npmjs.com/get-npm)
  * [Redis Commander](https://www.npmjs.com/package/redis-commander)
  * [DBeaver](https://dbeaver.io/)
  * [DB Browser for SQLite](https://sqlitebrowser.org/)
  * [SQLiteStudio](https://sqlitestudio.pl/)
  * Microsoft SQL Server Management Studio (Windows)
  * [ReportGenerator](https://github.com/danielpalme/ReportGenerator)
  * [python](https://www.python.org/downloads/windows/)

</details>

## Getting Started
<details>
  <summary>Building</summary>
</p>

1. clone repo
```bash
  git clone https://github.com/TrevorDArcyEvans/GraphML.git
```
1. build
```bash
  dotnet restore
  dotnet build
```
1. run tests
```bash
  dotnet test
```
1. run code coverage
```bash
  dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```
1. generate code coverage report
``` bash
  reportgenerator -reports:**/coverage.opencover.xml -targetdir:./CodeCoverage
```

</details>

<details>
  <summary>Back End</summary>
</p>

1. run _API_
```bash
  export ASPNETCORE_ENVIRONMENT=Development
  cd GraphML.API/bin/Debug/net5.0 
  ./GraphML.API
```
1. open [Swagger UI](https://localhost:5001/swagger/index.html)
1. start _Apache ActiveMQ_
1. start _Redis_
1. run _IdentityServer4_
```bash
  export ASPNETCORE_ENVIRONMENT=Development
  cd IdentityServerAspNetIdentity/bin/Debug/net5.0
  ./IdentityServerAspNetIdentity
```
1. open [IdentityServer4 Login](https://localhost:44387/Account/Login)
1. run _Analysis Server_
```bash
  export ASPNETCORE_ENVIRONMENT=Development
  cd GraphML.API/bin/Debug/net5.0 
  ./GraphML.Analysis.Server
```
1. open [_Apache ActiveMQ_ management console](http://localhost:8161/admin)
1. start _Redis Commander_
```bash
  redis-commander --port 8080
```
1. open [_Redis Commander_ management console](http://127.0.0.1:8080)

</details>

<details>
  <summary>Front End/s</summary>

### GraphML.UI.Web
```bash
  export ASPNETCORE_ENVIRONMENT=Development
  cd GraphML.UI.Web/bin/Debug/net5.0
  ./GraphML.UI.Web
```
open https://localhost:5002/

### GraphML.UI.Uno.UWP
* best to run from _Visual Studio_
* additional information:
  * [Command-Line Activation of Universal Windows Apps](https://blogs.windows.com/windowsdeveloper/2017/07/05/command-line-activation-universal-windows-apps/)
  * [Launching a Windows 10 UWP app from the command line cmd](https://stackoverflow.com/questions/51911405/launching-a-windows-10-uwp-app-from-the-command-line-cmd/51914388)

### GraphML.UI.Uno.Skia.Gtk
```bash
cd GraphML.UI.Uno/GraphML.UI.Uno.Skia.Gtk/bin/Debug/net5.0
./GraphML.UI.Uno.Skia.Gtk
```

### GraphML.UI.Uno.Wasm
```bash
cd GraphML.UI.Uno/GraphML.UI.Uno.Wasm/bin/Debug/net5.0/dist
python3 -m http.server 8000
```
open http://localhost:8000/

</details>

## Environment Variables

<details>
  <summary>Backend API</summary>
</p>

| Variable | Description | Example Value |
|----------|-------------|---------------|
| ASPNETCORE_ENVIRONMENT | ASP.NET Core runtime environment | `Production`, `Development`, `Test` |
||
| API_URI       | API server URL<p/>used by GraphML.API.Server to retrieve data |
||
| DATASTORE_CONNECTION         | | SqLite |
| DATASTORE_CONNECTION_TYPE    | | SqLite |
| DATASTORE_CONNECTION_STRING  | | Data Source=&#124;DataDirectory&#124;Data/GraphML.sqlite3; |
||
| LOG_CONNECTION_STRING | .NET connection string for database logging |
||
| RESULT_DATASTORE | _Redis_ URL | localhost:6379 |
||
| MESSAGE_QUEUE_URL               | _Apache ActiveMQ_ URL | activemq:tcp://localhost:61616 |
| MESSAGE_QUEUE_NAME              | | GraphML |
| MESSAGE_QUEUE_POLL_INTERVAL_S   | time in seconds between checking for new analysis jobs | 5 |
| MESSAGE_QUEUE_USE_THREADS       | | False |

</details>

<details>
  <summary>GraphML.UI.Uno.Wasm</summary>
</p>

We use a custom `index.html`:<br/>
  `GraphML:.\GraphML.UI.Uno\GraphML.UI.Uno.Wasm\wwwroot\index.html`

to load environment variables through:<br/>
  `GraphML:.\GraphML.UI.Uno\GraphML.UI.Uno.Wasm\WasmScripts\config-env-vars.js`

```javascript
//  How\where to configure BaseURL for Wasm app that uses WasmHttpHandler
//    https://github.com/unoplatform/uno/issues/1481#issuecomment-531480543
//  [wasm] Store AppSettings externally in some sort of editable text file such as
//          .config, .json or .xml, so these values can be changed depending the
//          on the deployment hosting target
//    https://github.com/unoplatform/uno/issues/1500
config.environmentVariables["IDENTITY_SERVER_CLIENT_ID"] = "GraphML.UI.Uno.Wasm";
config.environmentVariables["IDENTITY_SERVER_CLIENT_SECRET"] = "secret";
config.environmentVariables["API_URI"] = "https://localhost:5001";
```

</details>

## Overview
<details><p/>

  ![GraphML.Overview](Readme-Docs/GraphML.Overview.png "GraphML.Overview")

</details>

## Architecture
<details><p/>

  ![GraphML.Architecture](Readme-Docs/GraphML.Architecture.png "GraphML.Architecture")

</details>

## Analysis
<details><p/>

  ![GraphML.Analysis](Readme-Docs/GraphML.Analysis.Sequence.png "GraphML.Analysis")

</details>

## Data Model
<details>
  <summary>Classes</summary>

![GraphML.Classes](Readme-Docs/GraphML.Classes.png "GraphML.Classes")

</details>

<details>
  <summary>Composition</summary>

![GraphML.Composition](Readme-Docs/GraphML.Composition.png "GraphML.Composition")

</details>

<details>
  <summary>Description</summary>

<details>
  <summary>Base</summary>
  Abstract entities which are ancestors for other GraphML entities.

  * Item
    * Ultimate ancestor of all GraphML objects.
    * Models something which can be persisted.
    * Every item ultimately belongs to an Organisation
  * OwnedItem
    * Something which has an immediate owner, other than an Organisation

</details>

<details>
  <summary>Containers</summary>
  Entities which serve as a holding place for other entities.
  
  * Organisation
    * Typically a company, organisation or other legal entity in which people work together.
      * police force
      * GCHQ
      * FBI
      * military
      * bank
    * Used to isolate information between different Organisations
    * Id and OrganisationId **must** be the same
  * RepositoryManager
    * A means to group a subset of Repository in an Organisation in some logical manner.
    * For example, repositories could be grouped at a departmental level eg 'Financial Fraud' or 'Credit Control'.
    * ItemAttributeDefinition are held at RepositoryManager level so they can be shared across Repository.
  * Repository
    * A complete collection of Node and Edge representing an area of interest.
  * Graph
    * A subset of Nodes and Edges from a Repository which have been extracted for separate analysis.
    * A Graph may be directed; in contrast to a Repository, which has no notion of direction.
  * Chart
    * A 2D pictorial representation of a subset of Nodes and Edges from a Graph.
    * Generally used to visualise analysis results.
    * Layout algorithms can be applied to change the position of Nodes and Edges.

</details>

<details>
  <summary>Graph</summary>

  * RepositoryItem
    * Something which is in a Repository, either a Node or an Edge
  * Node
    * A vertex representing something of interest.
    * A Node may be connected to zero or one other Nodes by an Edge
    * A Node may have properties associated with it via an NodeItemAttribute
  * Edge
    * A link connecting two Node.
    * An Edge may have a 'weight/s' (or other properties) associated with it via an EdgeItemAttribute
    * An Edge is not directed 'per se'; this is set on the Graph
    <p/>
  * GraphItem
    * Something which is in a Graph, either a GraphNode or a GraphEdge
  * GraphNode
    * A Node which appears in a Graph.
    * Name may be different to that of underlying Node
  * GraphEdge
    * An Edge which appears in a Graph.
    * Name may be different to that of underlying Edge
    <p/>
  * ChartItem
    * Something which is in a Chart, either a ChartNode or a ChartEdge
  * ChartNode
    * A Node which appears in a Chart.
    * Name may be different to that of underlying Node
  * ChartEdge
    * An Edge which appears in a Chart.
    * Name may be different to that of underlying Edge

</details>

<details>
  <summary>Attributes</summary>
  ItemAttributeDefinition are held at RepositoryManager level so they can be shared across Repository.

  * ItemAttributeDefinition
    * Defines shape (name and data type) of information in an ItemAttribute
  * RepositoryItemAttributeDefinition
    * Defines shape of information in a RepositoryItemAttribute
  * GraphItemAttributeDefinition
    * Defines shape of information in a GraphItemAttribute
  * NodeItemAttributeDefinition
    * Defines shape of information in a NodeItemAttribute
  * EdgeItemAttributeDefinition
    * Defines shape of information in an EdgeItemAttribute
    <p/>
  * ItemAttribute
    * Additional information attached to an Item
  * RepositoryItemAttribute
    * Additional information attached to a Repository
  * GraphItemAttribute
    * Additional information attached to a Graph
  * NodeItemAttribute
    * Additional information attached to a Node
  * EdgeItemAttribute
    * Additional information attached to an Edge
    <p/>
  * Currently supported data types:
    * string
    * bool
    * int
    * double
    * DateTime (UTC)
    * DateInterval (UTC)

</details>

<details>
  <summary>Support</summary>

  * Contact
    * A person identified by their email address.
    * The email address (Name) is used to link authentication (IdentityServer4) to Role.
  * Role
    * The function performed by a Contact in the context of GraphML.
    * There are several, predefined functions in Roles
    * A Contact may have one or more Roles
  * Roles
    * User roles within GraphML

</details>

</details>

## Authentication & Authorisation

<details>
  <summary>Roles and Users</summary>

* enable `Development` mode by setting env var:  
```bash
  export ASPNETCORE_ENVIRONMENT=Development
```
* authentication (who you are) is handled by IdentityServer
* authorisation (what you can do) is handled by GraphML, based on an _email_ claim
* security is role based, with the following predefined roles:

| Role        | Description |
|-------------|-------------|
| User        | An entity using GraphML |
| UserAdmin   | An entity managing a subset of data within GraphML, typically data belonging to a single organisation |
| Admin       | An entity managing all data within GraphML |
* the above roles are owned by _System_ organisation
* SwaggerUI is only enabled in `Development` mode
* SwaggerUI authentication will redirect to a login screen in IdentityServer
* GraphML and IdentityServer4 have some test users:

| UserName | Password     | Email                           | Roles | Notes |
|----------|--------------|---------------------------------|-------|-------|
| `alice`  | `Pass123$`   | DrKool@KoolOrganisation.org     | Admin | system wide admin |
| `bob`    | `Pass123$`   | BobSmith@email.com              | none | known to _IdentityServer4_ but not _GraphML_ |
| `carol`  | `Pass123$`   | carol@KoolOrganisation.org      | UserAdmin |
| `dave`   | `Pass123$`   | dave@KoolOrganisation.org       | User |

</details>

<details>
  <summary>How to add a new user</summary>

* add user to _GraphML_
  * `GraphML:./GraphML.Datastore.Database/Data/Import.sql`
  * import into database
* add user to _IdentityServer4_
  * `GraphML:./IdentityServerAspNetIdentity/SeedData.cs`
  * import into database
    ```bash
    ./IdentityServerAspNetIdentity.exe /seed
    ```

</details>

</details>

## Multi-Tenancy
<details>

At this stage, multi-tenancy isolation is implemented in GraphML.Logic:
* GraphML.Logic.Validators
  * does the initial call even make sense
  * only allow calls on items which caller is allowed to access
* GraphML.Logic.Filters
  * only return items relevant to the caller
  * only return items caller is allowed to see

Future work will change to a database-per-client type of isolation
which is better suited to high security environments.
This will make validators and filters redundnant as all calls are
guaranteed to come from the same organisation.  In turn, this will
make the Organisation entity redundant.

</details>

## Misc
<details>
  <summary>Port Allocations</summary>

| Service | Port | Notes |
|---------|------|-------|
| IdentityServerAspnetIdentity | 44387 |
| GraphML.API | 5001 |
| GraphML.UI.Web | 5002 |
| GraphML.UI.Uno.Wasm | 8001 | when running from _Visual Studio_, port is set in:<p/>`GraphML:.\GraphML.UI.Uno\GraphML.UI.Uno.Wasm\Properties\launchSettings.json` |
| Apache ActiveMQ | 61616 |
| Apache ActiveMQ console | 8161 |
| Redis | 6379 |
| Redis Commander | 8080 | default port 8081
| Microsoft SQL Server | 1443 |
| MariaDB | 3306 |
| PostgreSQL | 5432 |

</details>

<details>
  <summary>Apache ActiveMQ</summary>

You can monitor ActiveMQ using the Web Console by pointing your browser at http://localhost:8161/admin .  
From ActiveMQ 5.8 onwards the web apps is secured out of the box.  
The default username and password is `admin/admin`.

</details>

<details>
  <summary>Redis</summary>

### Redis on Windows
Recommended method is to use a _Docker_ container:
```bash
  docker pull redis
  docker run -p 6379:6379 redis
```

Alternate method is to install and run Redis on WSL:

  https://redislabs.com/blog/redis-on-windows-10/

### Redis Commander
```bash
  npm install -g redis-commander
  redis-commander --port 8080
```
open [_Redis Commander_ management console](http://127.0.0.1:8080)

</details>

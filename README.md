# GraphML
<details>
TODO description
</details>

## Getting Started
<details>

1. clone repo
```bash
  git clone https://github.com/TrevorDArcyEvans/GraphML.git
```
1. generate self signed, https certificate
```bash
  cd GraphML.API
  ./CreateCert.sh
  cp GraphML.pfx Debug/netcoreapp3.1/
  cd ..
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
1. run API
```bash
  export ASPNETCORE_ENVIRONMENT=Development
  cd GraphML.API/bin/Debug/netcoreapp3.1 
  ./GraphML.API
```
1. open [Swagger UI](http://localhost:5000)

</details>

## Environment Variables
<details>

### Backend API
<details>

|Variable | Description | Example Value|
|---------|-------|--------------|
ASPNETCORE_ENVIRONMENT | ASP.NET Core runtime environment | `Production`, `Development`, `Test`
||
API_URI       | |
API_USERNAME  | |
API_PASSWORD  | |
||
DATASTORE_CONNECTION         | | SqLite
DATASTORE_CONNECTION_TYPE    | | SqLite
DATASTORE_CONNECTION_STRING  | | Data Source=&#124;DataDirectory&#124;Data/GraphML.sqlite3;
||
LOG_CONNECTION_STRING | |
LOG_BEARER_AUTH       | | False
||
OIDC_USERINFO_URL | |
OIDC_ISSUER_URL   | |
OIDC_AUDIENCE     | |
||
RESULT_DATASTORE | | localhost:6379
||
KESTREL_CERTIFICATE_FILENAME  | | GraphML.pfx
KESTREL_CERTIFICATE_PASSWORD  | | DisruptTheMarket
KESTREL_URLS                  | | http://localhost:5000
KESTREL_HTTPS_PORT            | | 8000
||
MESSAGE_QUEUE_URL               | | activemq:tcp://localhost:61616
MESSAGE_QUEUE_NAME              | | GraphML
MESSAGE_QUEUE_POLL_INTERVAL_S   | | 5
MESSAGE_QUEUE_USE_THREADS       | | False

</details>
</details>

## Architecture
<details>
TODO
</details>

## Data Model
<details>
TODO
</details>

## Notes
<details>

* enable `Development` mode by setting env var:  
&nbsp;&nbsp;&nbsp;&nbsp;  `export ASPNETCORE_ENVIRONMENT=Development`
* SwaggerUI is only enabled in `Development` mode
* Basic authentication (username/password) is only enabled in `Development` mode
* Basic authentication is `username`=`password` eg `Admin/Admin`
* For basic authentication, `role`=`username`

</details>

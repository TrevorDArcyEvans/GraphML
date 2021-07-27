FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

COPY . .
WORKDIR /src/GraphML.Analysis.Server
RUN dotnet restore GraphML.Analysis.Server.csproj
RUN dotnet build GraphML.Analysis.Server.csproj -c Debug -o /app

FROM build AS publish
RUN dotnet publish -c Debug -o /app/publish
WORKDIR /workdir

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GraphML.Analysis.Server.dll"]

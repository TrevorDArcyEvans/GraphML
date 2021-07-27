FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

COPY . .
WORKDIR /src/GraphML.API
RUN dotnet restore GraphML.API.csproj
RUN dotnet build GraphML.API.csproj -c Debug -o /app

FROM build AS publish
RUN dotnet publish -c Debug -o /app/publish
WORKDIR /workdir

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GraphML.API.dll"]

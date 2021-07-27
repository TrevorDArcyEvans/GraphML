FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore IdentityServerAspNetIdentity.csproj
RUN dotnet build IdentityServerAspNetIdentity.csproj -c Debug -o /app

FROM build AS publish
RUN dotnet publish -c Debug -o /app/publish
WORKDIR /workdir

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdentityServerAspNetIdentity.dll"]

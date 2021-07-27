FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

# manually install TypeScript:
#   https://semradj.wordpress.com/2020/08/29/asp-net-core-typescript-in-docker/
RUN apt update -y && apt install -y curl
RUN curl -sL https://deb.nodesource.com/setup_14.x | bash -
RUN apt-get install -y nodejs

COPY . .
WORKDIR /src/GraphML.UI.Web
RUN dotnet restore GraphML.UI.Web.csproj
RUN dotnet build GraphML.UI.Web.csproj -c Debug -o /app

FROM build AS publish
RUN dotnet publish -c Debug -o /app/publish
WORKDIR /workdir

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GraphML.UI.Web.dll"]

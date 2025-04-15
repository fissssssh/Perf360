FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-server
WORKDIR /src

# copy csproj and restore as distinct layers
COPY src/Perf360.Server/*.sln .
COPY src/Perf360.Server/Perf360.Server/*.csproj ./Perf360.Server/
RUN dotnet restore

# copy everything else and build app
COPY src/Perf360.Server/. ./Perf360.Server/
WORKDIR /src/Perf360.Server/Perf360.Server
RUN dotnet publish -c release -o /app

FROM node:23.10.0-alpine AS build-webapp
WORKDIR /src
COPY src/Perf360/package* .
RUN npm i
COPY src/Perf360/. .
RUN npm run build

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build-server /app ./
COPY --from=build-webapp /src/dist/. ./wwwroot/
ENTRYPOINT ["dotnet", "Perf360.Server.dll"]
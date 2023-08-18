#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/nightly/sdk:8.0-preview AS base
WORKDIR /app
EXPOSE 10350
ENV ASPNETCORE_URLS="http://*:10350"
ENV ASPNETCORE_ENVIRONMENT="Debug"
FROM base AS build
WORKDIR /src
COPY ["ETHTPS.HangfireRunner/ETHTPS.TaskRunner.csproj", "ETHTPS.HangfireRunner/"]
COPY ["ETHTPS.Configuration/ETHTPS.Configuration.csproj", "ETHTPS.Configuration/"]
COPY ["ETHTPS.Data.Database/ETHTPS.Data.Integrations.MSSQL.csproj", "ETHTPS.Data.Database/"]
COPY ["ETHTPS.Data/ETHTPS.Data.Core.csproj", "ETHTPS.Data/"]
COPY ["ETHTPS.DependencyInjection/ETHTPS.API.DependencyInjection.csproj", "ETHTPS.DependencyInjection/"]
COPY ["ETHTPS.API.Core.Integrations/ETHTPS.API.Core.Integrations.MSSQL.csproj", "ETHTPS.API.Core.Integrations/"]
COPY ["ETHTPS.Data.InfluxIntegration/ETHTPS.Data.Integrations.InfluxIntegration.csproj", "ETHTPS.Data.InfluxIntegration/"]
COPY ["ETHTPS.BIL/ETHTPS.API.BIL.csproj", "ETHTPS.BIL/"]
COPY ["ETHTPS.API.Core/ETHTPS.API.Core.csproj", "ETHTPS.API.Core/"]
COPY ["ETHTPS.Services.BackgroundTasks/ETHTPS.Services.BackgroundTasks.csproj", "ETHTPS.Services.BackgroundTasks/"]
COPY ["ETHTPS.BackgroundServices/ETHTPS.Services.csproj", "ETHTPS.BackgroundServices/"]
COPY ["ETHTPS.Services.Ethereum/ETHTPS.Services.Ethereum.csproj", "ETHTPS.Services.Ethereum/"]
COPY ["ETHTPS.Security/ETHTPS.API.Security.Core.csproj", "ETHTPS.Security/"]
RUN dotnet restore "ETHTPS.HangfireRunner/ETHTPS.TaskRunner.csproj"
COPY . .
WORKDIR /src
COPY [".env.development", "/src/"]
WORKDIR "/src/ETHTPS.HangfireRunner"
RUN dotnet build "ETHTPS.TaskRunner.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "ETHTPS.TaskRunner.csproj" -c Debug -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ETHTPS.TaskRunner.dll"]
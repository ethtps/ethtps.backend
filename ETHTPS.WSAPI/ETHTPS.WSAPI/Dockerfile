#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/nightly/sdk:8.0-preview AS base
USER app
WORKDIR /app
EXPOSE 5136

FROM mcr.microsoft.com/dotnet/nightly/sdk:8.0-preview AS build
WORKDIR /src
COPY ["ETHTPS.WSAPI/ETHTPS.WSAPI/ETHTPS.WSAPI.csproj", "ETHTPS.WSAPI/ETHTPS.WSAPI/"]
COPY ["ETHTPS.DependencyInjection/ETHTPS.API.DependencyInjection.csproj", "ETHTPS.DependencyInjection/"]
COPY ["ETHTPS.API.Core.Integrations/ETHTPS.API.Core.Integrations.MSSQL.csproj", "ETHTPS.API.Core.Integrations/"]
COPY ["ETHTPS.Data.Database/ETHTPS.Data.Integrations.MSSQL.csproj", "ETHTPS.Data.Database/"]
COPY ["ETHTPS.Data/ETHTPS.Data.Core.csproj", "ETHTPS.Data/"]
COPY ["ETHTPS.Core/ETHTPS.Core.csproj", "ETHTPS.Core/"]
COPY ["ETHTPS.Utils/ETHTPS.Utils.csproj", "ETHTPS.Utils/"]
COPY ["ETHTPS.Data.InfluxIntegration/ETHTPS.Data.Integrations.InfluxIntegration.csproj", "ETHTPS.Data.InfluxIntegration/"]
COPY ["ETHTPS.BIL/ETHTPS.API.BIL.csproj", "ETHTPS.BIL/"]
COPY ["ETHTPS.Configuration/ETHTPS.Configuration.csproj", "ETHTPS.Configuration/"]
COPY ["ETHTPS.API.Core/ETHTPS.API.Core.csproj", "ETHTPS.API.Core/"]
COPY ["ETHTPS.Services.BackgroundTasks/ETHTPS.Services.BackgroundTasks.csproj", "ETHTPS.Services.BackgroundTasks/"]
COPY ["ETHTPS.BackgroundServices/ETHTPS.Services.csproj", "ETHTPS.BackgroundServices/"]
COPY ["ETHTPS.Services.Ethereum/ETHTPS.Services.Ethereum.csproj", "ETHTPS.Services.Ethereum/"]
COPY ["ETHTPS.Security/ETHTPS.API.Security.Core.csproj", "ETHTPS.Security/"]
RUN dotnet restore "ETHTPS.WSAPI/ETHTPS.WSAPI/ETHTPS.WSAPI.csproj"
COPY . .
WORKDIR "/src/ETHTPS.WSAPI/ETHTPS.WSAPI"
RUN dotnet build "ETHTPS.WSAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ETHTPS.WSAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ETHTPS.WSAPI.dll"]
#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 10202
ENV ASPNETCORE_URLS=http://+:10202
ENV ASPNETCORE_ENVIRONMENT=”development”

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ETHTPS.API/ETHTPS.API.csproj", "ETHTPS.API/"]
COPY ["ETHTPS.API.Core/ETHTPS.API.Core.csproj", "ETHTPS.API.Core/"]
COPY ["ETHTPS.BIL/ETHTPS.API.BIL.csproj", "ETHTPS.BIL/"]
COPY ["ETHTPS.Data/ETHTPS.Data.Core.csproj", "ETHTPS.Data/"]
COPY ["ETHTPS.Data.Database/ETHTPS.Data.Integrations.MSSQL.csproj", "ETHTPS.Data.Database/"]
COPY ["ETHTPS.Services.BackgroundTasks/ETHTPS.Services.BackgroundTasks.csproj", "ETHTPS.Services.BackgroundTasks/"]
COPY ["ETHTPS.BackgroundServices/ETHTPS.Services.csproj", "ETHTPS.BackgroundServices/"]
COPY ["ETHTPS.Configuration/ETHTPS.Configuration.csproj", "ETHTPS.Configuration/"]
COPY ["ETHTPS.Data.InfluxIntegration/ETHTPS.Data.Integrations.InfluxIntegration.csproj", "ETHTPS.Data.InfluxIntegration/"]
COPY ["ETHTPS.Services.Ethereum/ETHTPS.Services.Ethereum.csproj", "ETHTPS.Services.Ethereum/"]
COPY ["ETHTPS.DependencyInjection/ETHTPS.API.DependencyInjection.csproj", "ETHTPS.DependencyInjection/"]
COPY ["ETHTPS.API.Core.Integrations/ETHTPS.API.Core.Integrations.MSSQL.csproj", "ETHTPS.API.Core.Integrations/"]
COPY ["ETHTPS.Security/ETHTPS.API.Security.Core.csproj", "ETHTPS.Security/"]
RUN dotnet restore "ETHTPS.API/ETHTPS.API.csproj"
COPY . .
WORKDIR "/src/ETHTPS.API"
RUN dotnet build "ETHTPS.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ETHTPS.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ETHTPS.API.dll"]
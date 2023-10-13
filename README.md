# ETHTPS.Backend

## Aggregated Ethereum TPS data for L2s and sidechains

[![.NET](https://github.com/ethtps/ethtps.backend/actions/workflows/dotnet.yml/badge.svg)](https://github.com/ethtps/ethtps.backend/actions/workflows/dotnet.yml)

The ASP.NET Core backend for [ethtps.info](https://ethtps.info)'s API

### Dependencies

1. [Dotnet 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

### Setup

1. Clone the repository and change directory

> `git clone https://github.com/ethtps/ethtps.backend.git; cd ethtps.backend`

2. Use the `prepare.sh` script to prepare the project for building

> `./prepare.sh dev`

Note: the `prepare.sh` script takes a single argument, `dev` or `prod`, which describes the environment to prepare for.

3. Build the project

> `dotnet build ETHTPS.API.sln`

4. Run the project

We're using a special project called `ETHTPS.Runner` project to run the API, the SignalR hub, and the background worker all at once while handling potential crashes. This is purely for convenience.

> `dotnet run --project ./ETHTPS.Runner/ETHTPS.Runner.csproj`

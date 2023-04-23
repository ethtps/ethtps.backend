# ETHTPS.Backend

## Aggregated Ethereum TPS data for L2s and sidechains

[![.NET](https://github.com/ethtps/ethtps.backend/actions/workflows/dotnet.yml/badge.svg)](https://github.com/ethtps/ethtps.backend/actions/workflows/dotnet.yml)

The ASP.NET Core backend for [ethtps.info](https://ethtps.info)'s API

### Setup

Note: For environment variables, I'm using `{var}` as a value YOU have to define. For example, if you have var `ENV=DEVLOPMENT` and `B_{ENV}=2`, you need to define `B_DEVELOPMENT`. I'm doing this so it's easier to switch between environments while developing.

1. Define the following environment variables: `ETHTPS_ENV`, `ETHTPS_BASE_DIR` (parent directory of the current repo), `ETHTPS_CONFIGURATION_PROVIDER_DB_CONN_STR`, `ETHTPS_API_DIR_{ETHTPS_ENV}`, `ETHTPS_WSAPI_DIR_{ETHTPS_ENV}`, `{ETHTPS_ENV}`, `EXPLORER` (`nautilus`/`dolphin` etc.), `ETHTPS_BACKEND=ethtps.backend`, `ETHTPS_OUT_DIR_{ENV}`, `ETHTPS_API_LIB_NAME=ETHTPS.API.dll`, `ETHTPS_RUNNER=ETHTPS.Runner`

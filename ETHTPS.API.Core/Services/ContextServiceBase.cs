﻿using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater;
using ETHTPS.Data.Core;
using ETHTPS.Data.Core.Models.DataUpdater;
using ETHTPS.Data.Core.Models.Providers;
using ETHTPS.Data.Integrations.MSSQL;
using ETHTPS.Data.ResponseModels;

using Microsoft.EntityFrameworkCore;

namespace ETHTPS.API.Core.Services
{
    public abstract class ContextServiceBase
    {
        protected EthtpsContext Context { get; private set; }
        private readonly IDataUpdaterStatusService _dataUpdaterStatusService;
        protected ContextServiceBase(EthtpsContext context, IDataUpdaterStatusService dataUpdaterStatusService)
        {
            Context = context;
            _dataUpdaterStatusService = dataUpdaterStatusService;
        }

        protected bool IsSidechain(string provider)
        {
            bool result = false;
            lock (Context.LockObj)
            {
                result = Providers().First(x => x.Name == provider).Type == "Sidechain";
            }
            return result;
        }

        protected IEnumerable<TimeInterval> TimeIntervals()
        {
            var excluded = new[] { "Instant", "Latest", "All", "Auto" };
            foreach (var interval in Enum.GetValues(typeof(TimeInterval)))
            {
                if (excluded.Contains(interval.ToString()))
                    continue;

                yield return (TimeInterval)interval;
            }
        }

        public IEnumerable<ProviderResponseModel> Providers()
        {
            IEnumerable<ProviderResponseModel> result;
            lock (Context.LockObj)
            {
                var types = Context.ProviderTypes.AsEnumerable();
                result = Context.Providers.Select(x => new ProviderResponseModel()
                {
                    Name = x.Name,
                    Type = Context.ProviderTypes.First(y=>y.Id == x.Type).Name,
                    Color = x.Color,
                    TheoreticalMaxTPS = x.TheoreticalMaxTps,
                    IsGeneralPurpose = types.First(y=>y.Id == x.Type).IsGeneralPurpose,
                    IsSubchainOf = "Ethereum",
                    Status = _dataUpdaterStatusService.GetStatusFor(x.Name, UpdaterType.BlockInfo),
                    Enabled = x.Enabled
                }).Where(x => x.Enabled);
            }
            return result;
        }
    }
}

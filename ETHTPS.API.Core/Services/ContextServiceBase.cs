using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater;
using ETHTPS.Data.Core;
using ETHTPS.Data.Core.Extensions;
using ETHTPS.Data.Core.Models.DataUpdater;
using ETHTPS.Data.Core.Models.ResponseModels;
using ETHTPS.Data.Integrations.MSSQL;

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
                result = AllProviders.First(x => x.Name == provider).Type == "Sidechain";
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

        private IEnumerable<ProviderResponseModel> _allProviders = Enumerable.Empty<ProviderResponseModel>();

        public IEnumerable<ProviderResponseModel> AllProviders
        {
            get
            {
                if (_allProviders.Count() == 0)
                {
                    lock (Context.LockObj)
                    {
                        var types = Context.ProviderTypes.AsEnumerable();
                        _allProviders = Context.Providers.ToList().SafeSelect(x => new ProviderResponseModel()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Type = types.First(y => y.Id == x.Type).Name,
                            Color = x.Color,
                            TheoreticalMaxTPS = x.TheoreticalMaxTps,
                            IsGeneralPurpose = types.First(y => y.Id == x.Type).IsGeneralPurpose,
                            IsSubchainOf = "Ethereum",
                            Status = _dataUpdaterStatusService.GetStatusFor(x.Name, UpdaterType.BlockInfo) ?? new()
                            {
                                Status = "Failed"
                            },
                            Enabled = x.Enabled
                        }).SafeWhere(x => x.Enabled).ToList();
                    }
                }
                return _allProviders;

            }
        }
    }
}

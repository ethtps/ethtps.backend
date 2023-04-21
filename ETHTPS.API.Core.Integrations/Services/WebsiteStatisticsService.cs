using ETHTPS.API.BIL.Infrastructure.Services;
using ETHTPS.Data.Integrations.MSSQL;
using ETHTPS.Data.Integrations.MSSQL.Extensions;

namespace ETHTPS.API.Core.Integrations.MSSQL.Services
{
    public class WebsiteStatisticsService : IWebsiteStatisticsService
    {
        private readonly EthtpsContext _context;
        const string _NAME = "CurrentVisitors";
        public bool Enabled { get => _context.Experiments.Any(x => x.Name == "Current visitors"); }
        public WebsiteStatisticsService(EthtpsContext context)
        {
            _context = context;

            if (!_context.CachedResponses.Get<int?, CachedResponse>(_NAME).HasValue)
            {
                _context.CachedResponses.Add(new()
                {
                    KeyJson = _NAME,
                    Name = _NAME,
                    ValueJson = "0"
                });
                _context.SaveChanges();
            }
        }

        public int GetNumberOfCurrentVisitors()
        {
            lock (_context.LockObj)
            {
                return _context.CachedResponses.Get<int, CachedResponse>(_NAME);
            }
        }

        public void SetNumberOfCurrentVisitors(int count)
        {
            lock (_context.LockObj)
            {
                var entry = _context.CachedResponses.First(c => c.Name == _NAME);
                entry.ValueJson = count.ToString();
                _context.Update(entry);
                _context.SaveChanges();
            }
        }

        public void IncrementNumberOfCurrentVisitors() => SetNumberOfCurrentVisitors(GetNumberOfCurrentVisitors() + 1);

        public void DecrementNumberOfCurrentVisitors() => SetNumberOfCurrentVisitors(GetNumberOfCurrentVisitors() - 1);
    }
}

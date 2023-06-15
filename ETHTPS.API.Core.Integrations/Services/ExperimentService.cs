using ETHTPS.API.BIL.Infrastructure.Services;
using ETHTPS.Data.Core.Extensions;
using ETHTPS.Data.Core.Models;
using ETHTPS.Data.Integrations.MSSQL;
using ETHTPS.Data.Integrations.MSSQL.Extensions;

using Microsoft.AspNetCore.Http;

namespace ETHTPS.API.Core.Integrations.MSSQL.Services
{
    public sealed class ExperimentService : IExperimentService
    {
        private readonly EthtpsContext _context;

        public ExperimentService(EthtpsContext context)
        {
            _context = context;
        }

        public Task EnrollInNewExperimentsIfApplicableAsync(ExperimentRequesterParameters parameters, HttpContext context)
        {
            var experiments = _context.GetExperimentsForDeviceType(parameters.DeviceType).ToList();
            var runningExperiments = experiments.Where(x => x.IsRunning()).ToList();
            if (runningExperiments.Any())
            {
                foreach (var experiment in runningExperiments)
                {
                    var apiKeyID = _context.GetAPIKeyID(context);
                    if (_context.UserIsEligibleForEnrollmentIn(experiment, apiKeyID))
                    {
                        _context.EnrollUserIn(experiment, apiKeyID);
                    }
                }
            }
            return Task.CompletedTask;
        }


        public async Task<IEnumerable<int>> GetAvailableExperimentsAsync(ExperimentRequesterParameters parameters, HttpContext context)
        {
            await EnrollInNewExperimentsIfApplicableAsync(parameters, context);
            if (!string.IsNullOrWhiteSpace(parameters.DeviceType))
            {
                lock (_context.LockObj)
                {
                    return _context.GetExperimentsUserIsEnrolledIn(_context.GetAPIKeyID(context)).SafeSelect(x => x != null ? x.Id : -1);
                }
            }
            return Enumerable.Empty<int>();
        }

        public void GiveAnonymousFeedback()
        {
            throw new System.NotImplementedException();
        }
    }
}

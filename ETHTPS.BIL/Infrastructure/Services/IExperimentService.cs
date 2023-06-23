
using ETHTPS.Data.Core.Models;
using ETHTPS.Data.Integrations.MSSQL;

using Microsoft.AspNetCore.Http;

namespace ETHTPS.API.BIL.Infrastructure.Services
{
    public interface IExperimentService
    {
        public Task<IEnumerable<int>> GetAvailableExperimentsAsync(ExperimentRequesterParameters parameters, HttpContext context);
        public Task EnrollInNewExperimentsIfApplicableAsync(ExperimentRequesterParameters parameters, HttpContext context);
        public Task<Experiment?> GetExperimentByIDAsync(int id);
        public void GiveAnonymousFeedback(ExperimentFeedback feedback);
    }
}

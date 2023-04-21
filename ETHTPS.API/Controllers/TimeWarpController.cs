using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ETHTPS.API.Core.Integrations.MSSQL.Services;
using ETHTPS.Data.Core.Models.DataPoints;
using ETHTPS.Data.Core.Models.Queries.Data.Requests;
using ETHTPS.Data.Integrations.MSSQL.TimeWarp;
using ETHTPS.Data.Integrations.MSSQL.TimeWarp.Models;

using Microsoft.AspNetCore.Mvc;

namespace ETHTPS.API.Controllers
{
    [Route("/api/v3/TimeWarp/[action]")]
    public class TimeWarpController
    {
        private readonly TimeWarpService _timeWarpService;
        private const string _DEFAULT_SMOOTHING = "Instant";
        public TimeWarpController(TimeWarpService timeWarpService)
        {
            _timeWarpService = timeWarpService;
        }

        [HttpGet]
        public DateTime GetEarliestDate()
        {
            return ((ITimeWarpService)_timeWarpService).GetEarliestDate();
        }

        [HttpGet]
        public IEnumerable<DataPoint> GetGasAdjustedTPSAt(ProviderQueryModel model, long timestamp, string smoothing = _DEFAULT_SMOOTHING, int count = 30)
        {
            return ((ITimeWarpService)_timeWarpService).GetGasAdjustedTPSAt(model, timestamp, smoothing, count);
        }

        [HttpGet]
        public IEnumerable<DataPoint> GetGPSAt(ProviderQueryModel model, long timestamp, string smoothing = _DEFAULT_SMOOTHING, int count = 30)
        {
            return ((ITimeWarpService)_timeWarpService).GetGPSAt(model, timestamp, smoothing, count);
        }

        [HttpGet]
        public Task<TimeWarpSyncProgressModel> GetSyncProgress(ProviderQueryModel model)
        {
            return ((ITimeWarpService)_timeWarpService).GetSyncProgress(model);
        }

        [HttpGet]
        public IEnumerable<DataPoint> GetTPSAt(ProviderQueryModel model, long timestamp, string smoothing = _DEFAULT_SMOOTHING, int count = 30)
        {
            return ((ITimeWarpService)_timeWarpService).GetTPSAt(model, timestamp, smoothing, count);
        }

    }
}

using System;
using System.Collections.Generic;

using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater;
using ETHTPS.API.Core.Controllers;
using ETHTPS.Data.Core.Models.DataUpdater;

using Microsoft.AspNetCore.Mvc;

namespace ETHTPS.API.Controllers
{
    [Route("/api/v3/updater-status/[action]")]
    [ApiController]
    public class UpdaterStatusController : APIControllerBase
    {
        private readonly IDataUpdaterStatusGetter _dataUpdaterService;

        public UpdaterStatusController(IDataUpdaterStatusService dataUpdaterService)
        {
            _dataUpdaterService = dataUpdaterService;
        }

        [HttpGet]
        public IEnumerable<LiveDataUpdaterStatus> GetAllStatuses()
        {
            return _dataUpdaterService.GetAllStatuses();
        }

        [HttpGet]
        public LiveDataUpdaterStatus GetStatusFor(string provider, string updaterType)
        {
            return _dataUpdaterService.GetStatusFor(provider, Enum.Parse<UpdaterType>(updaterType));
        }

        [HttpGet]
        public IEnumerable<LiveDataUpdaterStatus> GetStatusesFor(string provider)
        {
            return _dataUpdaterService.GetStatusFor(provider);
        }
    }
}

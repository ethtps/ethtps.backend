using System;
using System.Collections.Generic;

using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater;
using ETHTPS.API.Core.Attributes;
using ETHTPS.API.Core.Controllers;
using ETHTPS.Data.Core.Models.DataUpdater;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETHTPS.API.Controllers
{
    [Route("/api/v3/updater-status/[action]")]
    [ApiController]
    [Authorize]
    public sealed class UpdaterStatusController : APIControllerBase
    {
        private readonly IDataUpdaterStatusGetter _dataUpdaterService;

        public UpdaterStatusController(IDataUpdaterStatusService dataUpdaterService)
        {
            _dataUpdaterService = dataUpdaterService;
        }

        [HttpGet]
        [TTL(2)]
        public IEnumerable<LiveDataUpdaterStatus> GetAllStatuses()
        {
            return _dataUpdaterService.GetAllStatuses();
        }

        [HttpGet]
        [TTL(2)]
        public LiveDataUpdaterStatus GetStatusFor(string provider, string updaterType)
        {
            return _dataUpdaterService.GetStatusFor(provider, Enum.Parse<UpdaterType>(updaterType));
        }

        [HttpGet]
        [TTL(2)]
        public IEnumerable<LiveDataUpdaterStatus> GetStatusesFor(string provider)
        {
            return _dataUpdaterService.GetStatusFor(provider);
        }
    }
}

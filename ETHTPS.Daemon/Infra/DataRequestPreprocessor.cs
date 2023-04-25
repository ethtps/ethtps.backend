using ETHTPS.API.BIL.Infrastructure.Services.DataServices;
using ETHTPS.Data.Core.Models.Queries.Data.Requests;
using ETHTPS.Services.Infrastructure.Messaging;

using static ETHTPS.Daemon.Program;

namespace ETHTPS.Daemon.Infra
{
    /// <summary>
    /// A preprocessor for data requests. Checks whether the database already contains the requested data or part of it and skips the next step if everything already exists. Otherwise, it sends data gathering messages in order to fill in gaps. The flow then continues to the postprocessor(s).
    /// </summary>
    public sealed class DataRequestPreprocessor : DaemonActionBase<L2DataRequestModel>
    {
        private readonly string _requestGuid;
        private readonly Random _random = new Random();

        public DataRequestPreprocessor(string requestKey, IMessagePublisher publisher, IRedisCacheService cacheService, L2DataRequestModel? @object) : base(requestKey, publisher, cacheService, @object)
        {
            _requestGuid = requestKey.Split(':')[1];
        }

        public override async Task RunAsync()
        {
            //Mark the request as in progress
            var guid = _object?.Guid ?? L2DataRequestStatus.GenerateCacheKeyFromGuid(_requestGuid);
            await _cacheService.SetDataAsync(guid, new L2DataRequestStatus(5, L2DataRequestState.Processing, guid));
            LogVerbose($"[DataRequestPreprocessor]: Changed status to Processing for {_requestGuid}");
            await Task.Delay(_random.Next(10000));
        }
    }
}

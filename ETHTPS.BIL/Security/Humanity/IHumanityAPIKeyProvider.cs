using ETHTPS.API.BIL.Infrastructure.Services.APIKeys;

namespace ETHTPS.API.BIL.Security.Humanity
{
    public interface IHumanityAPIKeyProvider<THumanityCheckService> : IExtendedAPIKeyService
        where THumanityCheckService : IHumanityCheckService
    {
        THumanityCheckService HumanityService { get; }
    }
}

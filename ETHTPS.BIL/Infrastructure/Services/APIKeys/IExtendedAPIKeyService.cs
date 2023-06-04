using ETHTPS.Data.Core.Models.ResponseModels.APIKey;

namespace ETHTPS.API.BIL.Infrastructure.Services.APIKeys
{
    public interface IExtendedAPIKeyService : IAPIKeyService
    {
        Task<APIKeyResponseModel> RegisterNewKeyAsync(string humanityProof, string ipAddress);
        Task<APIKeyResponseModel> RegisterNewKeyForIPAddressAsync(string ipAddress);
    }
}

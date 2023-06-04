using ETHTPS.Data.Core.Models.ResponseModels.APIKey;

namespace ETHTPS.API.BIL.Infrastructure.Services.APIKeys
{
    public interface IAPIKeyService
    {
        Task<APIKeyResponseModel> RegisterNewKeyForProofAsync(string humanityProof);
    }
}

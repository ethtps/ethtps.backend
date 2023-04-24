using ETHTPS.API.BIL.Infrastructure.Services;
using ETHTPS.Data.Integrations.MSSQL;

namespace ETHTPS.API.Core.Integrations.MSSQL.Services
{
    public sealed class ProvidersService : EFCoreCRUDServiceBase<Provider>, IProvidersService<Provider>
    {
        public ProvidersService(EthtpsContext context) : base(context.Providers, context)
        {
        }
    }
}

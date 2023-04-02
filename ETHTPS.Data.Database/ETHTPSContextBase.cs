using ETHTPS.Data.Core.Database;
using ETHTPS.Data.Core.Models.Providers;

using Microsoft.EntityFrameworkCore;

namespace ETHTPS.Data.Integrations.MSSQL
{
    public abstract class ETHTPSContextBase: ContextBase<EthtpsContext>
    {
        public ETHTPSContextBase()
        {

        }

        public ETHTPSContextBase(DbContextOptions<EthtpsContext> options)
            : base(options) 
        {

        }
    }
}

using ETHTPS.Data.Core.Database;
using ETHTPS.Data.Core.Database.Relational;

using Microsoft.EntityFrameworkCore;

namespace ETHTPS.Data.Integrations.MSSQL
{
    public abstract class ETHTPSContextBase : ContextBase<EthtpsContext>, IRelationalDatabase
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

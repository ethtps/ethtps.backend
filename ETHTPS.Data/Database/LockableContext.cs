using Microsoft.EntityFrameworkCore;

namespace ETHTPS.Data.Core.Database
{
    public abstract class LockableContext : DbContext
    {
        public readonly object LockObj = new();

        protected LockableContext() { }
        protected LockableContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLazyLoadingProxies().EnableThreadSafetyChecks();
        }
    }
}

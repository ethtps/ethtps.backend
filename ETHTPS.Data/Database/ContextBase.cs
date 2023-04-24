using System;

using Microsoft.EntityFrameworkCore;

namespace ETHTPS.Data.Core.Database
{
    public abstract class ContextBase<TContext> : DbContext
        where TContext : DbContext
    {
        public readonly object LockObj = new object();
        public ContextBase()
        {
            Database.SetCommandTimeout(TimeSpan.FromSeconds(10));
        }

        public ContextBase(DbContextOptions<TContext> options)
            : base(options)
        {
            Database.SetCommandTimeout(TimeSpan.FromSeconds(60));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLazyLoadingProxies().EnableThreadSafetyChecks();
        }

    }
}

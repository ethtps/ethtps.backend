using System;

using Microsoft.EntityFrameworkCore;

namespace ETHTPS.Data.Core.Database
{
    public abstract class ContextBase<TContext> : LockableContext
        where TContext : DbContext
    {
        protected ContextBase()
        {
            Database.SetCommandTimeout(TimeSpan.FromSeconds(10));
        }

        protected ContextBase(DbContextOptions<TContext> options)
            : base(options)
        {
            Database.SetCommandTimeout(TimeSpan.FromSeconds(60));
        }
    }
}

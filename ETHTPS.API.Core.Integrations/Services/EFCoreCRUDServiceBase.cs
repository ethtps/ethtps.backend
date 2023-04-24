﻿using ETHTPS.API.BIL.Infrastructure.Services;
using ETHTPS.Data.Core;
using ETHTPS.Data.Integrations.MSSQL;

using Microsoft.EntityFrameworkCore;

namespace ETHTPS.API.Core.Integrations.MSSQL.Services
{
    public abstract class EFCoreCRUDServiceBase<TEntity> :
        ICRUDService<TEntity> where TEntity : class, IIndexed

    {
        private readonly DbSet<TEntity> _entitySet;
        private readonly Action _saveChangesAction;
        private readonly object _lockObject;
        protected EFCoreCRUDServiceBase(DbSet<TEntity> entitySet, EthtpsContext context)
        {
            _entitySet = entitySet;
            _saveChangesAction = () => context.SaveChanges();
            _lockObject = context.LockObj;
        }

        public void Create(TEntity entity)
        {
            lock (_lockObject)
            {
                _entitySet.Add(entity);
                _saveChangesAction();
            }
        }

        public void DeleteById(int id)
        {
            lock (_lockObject)
            {
                var target = _entitySet.FirstOrDefault(e => e.Id == id);
                if (target == null)
                    return;

                _entitySet.Remove(target);
                _saveChangesAction();
            }
        }

        public IEnumerable<TEntity> GetAll()
        {
            lock (_lockObject)
            {
                return _entitySet.ToList();
            }
        }

        public TEntity GetById(int id)
        {
            lock (_lockObject)
            {
                return _entitySet.First(e => e.Id == id);
            }

        }
        public void Update(TEntity entity)
        {
            lock (_lockObject)
            {
                _entitySet.Update(entity);
                _saveChangesAction();
            }
        }
    }
}

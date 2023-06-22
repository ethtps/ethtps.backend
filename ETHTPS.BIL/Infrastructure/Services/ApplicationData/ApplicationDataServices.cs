using ETHTPS.Data.Core;

namespace ETHTPS.API.BIL.Infrastructure.Services.ApplicationData
{
    public interface IApplicationDataService<TEntity> : IAbstractApplicationDataService<TEntity, TEntity>
    where TEntity : class, IIndexed
    { }
    public interface IAbstractApplicationDataService<IEntity, TEntity> : ICRUDService<TEntity>
        where TEntity : class, IEntity, IIndexed
    { }
}

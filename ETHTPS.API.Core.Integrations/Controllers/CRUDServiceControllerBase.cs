using ETHTPS.API.BIL.Infrastructure.Services;
using ETHTPS.API.Core.Controllers;
using ETHTPS.API.Core.Controllers.CRUD;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETHTPS.API.Core.Integrations.MSSQL.Controllers
{
    [Authorize]
    public abstract class CRUDServiceControllerBase<TEntity> : APIControllerBase, ICRUDController<TEntity> where TEntity : class
    {
        private readonly ICRUDService<TEntity> _serviceImplementation;
        public CRUDServiceControllerBase(ICRUDService<TEntity> serviceImplementation)
        {
            _serviceImplementation = serviceImplementation;
        }

        [Route("[action]")]
        [HttpPost]
        [Authorize(Policy = "EditorsOnly")]
        public void Create([FromBody] TEntity entity)
        {
            _serviceImplementation.Create(entity);
        }

        [Route("[action]")]
        [HttpDelete]
        [Authorize(Policy = "EditorsOnly")]
        public void DeleteById(int id)
        {
            _serviceImplementation.DeleteById(id);
        }

        [Route("[action]")]
        [HttpGet]
        public IEnumerable<TEntity> GetAll()
        {
            return _serviceImplementation.GetAll();
        }

        [HttpGet]
        [Route("[action]")]
        public TEntity GetById(int id)
        {
            return _serviceImplementation.GetById(id);
        }

        [Route("[action]")]
        [Authorize(Policy = "EditorsOnly")]
        [HttpPut]
        public void Update([FromBody] TEntity entity)
        {
            _serviceImplementation.Update(entity);
        }
    }
}

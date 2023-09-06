using ETHTPS.API.BIL.Infrastructure.Services;
using ETHTPS.API.Core.Attributes;
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
        protected CRUDServiceControllerBase(ICRUDService<TEntity> serviceImplementation)
        {
            _serviceImplementation = serviceImplementation;
        }

        [Route("[action]")]
        [HttpPost]
        // [Authorize(Policy = "EditorsOnly")]
        public void Create([FromBody] TEntity entity)
        {
            _serviceImplementation.Create(entity);
        }

        [Route("[action]")]
        [HttpDelete]
        // [Authorize(Policy = "EditorsOnly")]
        public void DeleteById(int id)
        {
            _serviceImplementation.DeleteById(id);
        }

        [Route("[action]")]
        [HttpGet]
        [TTL(30)]
        public IEnumerable<TEntity> GetAll()
        {
            return _serviceImplementation.GetAll();
        }

        [HttpGet]
        [Route("[action]")]
        [TTL(30)]
        public TEntity GetById(int id)
        {
            return _serviceImplementation.GetById(id);
        }

        [Route("[action]")]
        // [Authorize(Policy = "EditorsOnly")]
        [HttpPut]
        public void Update([FromBody] TEntity entity)
        {
            _serviceImplementation.Update(entity);
        }
    }
}

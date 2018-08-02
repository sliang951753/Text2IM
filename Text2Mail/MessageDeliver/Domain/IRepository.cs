using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageDeliver.Domain
{
    public interface IRepository<TEntity, TIdentify>
    {
        IEnumerable<TEntity> GetAll();

        TEntity GetById(TIdentify id);

        IEnumerable<TEntity> GetBySpecification(Specification<TEntity> spec);

        TEntity Add(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);
    }
}

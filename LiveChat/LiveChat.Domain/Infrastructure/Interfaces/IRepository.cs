using System.Collections.Generic;
using System.Linq;

namespace LiveChat.Domain.Infrastructure.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        void Save(TEntity entity);
        void Save(IEnumerable<TEntity> entity);
        void Delete(TEntity entity);
    }
}

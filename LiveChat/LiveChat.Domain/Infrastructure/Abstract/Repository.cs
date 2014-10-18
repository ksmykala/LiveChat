using LiveChat.Domain.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace LiveChat.Domain.Infrastructure.Abstract
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected DbSet<T> DbSet;
        private readonly DbContext _dbContext;

        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
            DbSet = _dbContext.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return DbSet;
        }

        public virtual void Save(T entity)
        {
            DbSet.Add(entity);
            _dbContext.SaveChanges();
        }

        public virtual void Save(IEnumerable<T> entities)
        {
            DbSet.AddRange(entities);
            _dbContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            DbSet.Remove(entity);
            _dbContext.SaveChanges();
        }
    }
}

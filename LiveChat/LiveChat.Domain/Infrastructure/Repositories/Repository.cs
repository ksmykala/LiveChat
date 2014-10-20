using LiveChat.Domain.Infrastructure.Interfaces;
using LiveChat.Domain.Models.EntityClasses;
using LiveChat.Domain.Models.EntityExtensions;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace LiveChat.Domain.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected DbSet<T> DbSet;
        private readonly LiveChatContext _dbContext = LiveChatContextSingleton.Instance;

        public Repository()
        {
            DbSet = _dbContext.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return DbSet;
        }

        public virtual void Save(T entity)
        {
            DbSet.AddOrUpdate(entity);
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

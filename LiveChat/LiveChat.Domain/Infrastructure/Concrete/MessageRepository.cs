using LiveChat.Domain.Infrastructure.Abstract;
using LiveChat.Domain.Models.EntityClasses;
using System;
using System.Data.Entity;
using System.Linq;

namespace LiveChat.Domain.Infrastructure.Concrete
{
    public class MessageRepository : Repository<Message>
    {
        public MessageRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<Message> GetLastMessages(int count)
        {
            return GetAll().Where(x => x.ConversationId == Guid.Empty).Take(count);
        }

        public override void Save(Message entity)
        {
            entity.Id = Guid.NewGuid();
            entity.CreateAt = DateTime.Now;
            base.Save(entity);
        }
    }
}

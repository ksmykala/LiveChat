using LiveChat.Domain.Models.EntityClasses;
using System;

namespace LiveChat.Domain.Infrastructure.Repositories
{
    public class MessageRepository : Repository<Message>
    {
        public override void Save(Message entity)
        {
            entity.Id = Guid.NewGuid();
            entity.CreateAt = DateTime.Now;
            base.Save(entity);
        }
    }
}

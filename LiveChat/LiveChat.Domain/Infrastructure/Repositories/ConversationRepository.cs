using LiveChat.Domain.Infrastructure.Interfaces;
using LiveChat.Domain.Models.EntityClasses;
using System;
using System.Linq;

namespace LiveChat.Domain.Infrastructure.Repositories
{
    public class ConversationRepository : Repository<Conversation>, IConversationRepository
    {
        public Conversation GetById(Guid id)
        {
            var result = GetAll().SingleOrDefault(x => x.Id == id);
            return result;
        }
    }
}

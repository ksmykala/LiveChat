using LiveChat.Domain.Models.EntityClasses;
using System;
using System.Collections.Generic;

namespace LiveChat.Domain.Infrastructure.Interfaces
{
    public interface IConversationRepository : IRepository<Conversation>
    {
        Conversation GetById(Guid id);
    }
}

using System.Linq;
using LiveChat.Domain.Models.EntityClasses;
using System;
using System.Collections.Generic;

namespace LiveChat.Domain.Infrastructure.Interfaces
{
    public interface IUsersInConversationsRepository : IRepository<UsersInConversation>
    {
        IQueryable<UsersInConversation> GetById(Guid id);
        Guid GetConversationForUsers(IList<User> users);
        IQueryable<int> GetUsersIdsForConversation(Guid conversationId);
    }
}

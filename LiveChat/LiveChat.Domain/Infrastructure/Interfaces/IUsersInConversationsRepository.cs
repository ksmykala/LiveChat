using LiveChat.Domain.Models.EntityClasses;
using System;
using System.Collections.Generic;

namespace LiveChat.Domain.Infrastructure.Interfaces
{
    public interface IUsersInConversationsRepository : IRepository<UsersInConversation>
    {
        Guid GetConversationForUsers(IList<User> users);
    }
}

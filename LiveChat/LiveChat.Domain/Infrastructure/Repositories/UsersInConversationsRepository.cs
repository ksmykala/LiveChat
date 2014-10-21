using LiveChat.Domain.Common.Helpers;
using LiveChat.Domain.Infrastructure.Interfaces;
using LiveChat.Domain.Models.EntityClasses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LiveChat.Domain.Infrastructure.Repositories
{
    public class UsersInConversationsRepository : Repository<UsersInConversation>, IUsersInConversationsRepository
    {
        public IQueryable<UsersInConversation> GetById(Guid id)
        {
            var result = GetAll().Where(x => x.ConversationId == id);
            return result;
        }

        public Guid GetConversationForUsers(IList<User> users)
        {
            var result = Guid.Empty;
            var userIds = users.Select(x => x.UserId)
                .OrderBy(x => x)
                .ToArray();
            var conversations = GetAll()
                .GroupBy(x => x.ConversationId, x => x.UserId,
                    (key, g) => new { ConversationId = key, UsersIds = g.OrderBy(o => o).ToList() });

            foreach (var conversation in conversations)
            {
                if (ComparerHelper.ArraysEqual(conversation.UsersIds.ToArray(), userIds))
                { 
                    result = conversation.ConversationId;
                    break;
                }
            }

            if (result == Guid.Empty)
                result = CreateNewConversation(users);

            return result;
        }

        public IQueryable<int> GetUsersIdsForConversation(Guid conversationId)
        {
            var result = GetById(conversationId).Select(x => x.UserId).OrderBy(x => x);
            return result;
        }

        private Guid CreateNewConversation(ICollection<User> users)
        {
            var conversationId = Guid.NewGuid();
            foreach (var user in users)
            {
                var userInConversation = new UsersInConversation
                {
                    Id = Guid.NewGuid(),
                    ConversationId = conversationId,
                    UserId = user.UserId
                };

                Save(userInConversation);
            }

            return conversationId;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using LiveChat.Domain.Common.Helpers;
using LiveChat.Domain.Infrastructure.Interfaces;
using LiveChat.Domain.Models.EntityClasses;

namespace LiveChat.Domain.Infrastructure.Repositories
{
    public class ConversationRepository : Repository<Conversation>, IConversationRepository
    {
        public Conversation GetById(Guid id)
        {
            var result = GetAll().SingleOrDefault(x => x.Id == id);
            return result;
        }

        public Conversation GetConversationForUsers(IList<User> users)
        {
            Conversation result = null;
            var userIds = users.Select(x => x.UserId).OrderBy(x => x).ToArray();
            var conversations = GetAll()
                    .Select(x => new
                    {
                        x.Id, 
                        UsersIds = x.Users.Select(y => y.UserId).OrderBy(z => z)
                    })
                    .ToList();

            foreach (var conversation in conversations)
            {
                if (ComparerHelper.ArraysEqual(conversation.UsersIds.ToArray(), userIds))
                { 
                    result = GetById(conversation.Id);
                    break;
                }
            }

            if(result == null)
                CreateNewConversation(users);

            return result;
        }

        private Conversation CreateNewConversation(ICollection<User> users)
        {
            var conversation = new Conversation
            {
                Id = Guid.NewGuid(),
                Users = users
            };

            Save(conversation);

            return conversation;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using LiveChat.Domain.Infrastructure.Interfaces;
using LiveChat.Domain.Models.EntityClasses;
using Microsoft.AspNet.SignalR;
using WebMatrix.WebData;

namespace LiveChat.App.Hubs
{
    public class PrivateChatHub : Hub
    {
        private readonly IRepository<Message> _messageRepository;
        private readonly IUsersInConversationsRepository _usersInConversationRepository;
        private readonly IUserRepository _usersRepository;

        public PrivateChatHub(IRepository<Message> messageRepository, IUsersInConversationsRepository conversationRepository, IUserRepository usersRepository)
        {
            _messageRepository = messageRepository;
            _usersInConversationRepository = conversationRepository;
            _usersRepository = usersRepository;
        }

        public void Send(Guid conversationId, string message)
        {
            var userIds = _usersInConversationRepository.GetUsersIdsForConversation(conversationId)
                    .ToList();

            var usersNames = _usersRepository.GetAll().Where(x => userIds.Contains(x.UserId)).ToList().Select(x => x.UserName).ToList();

            var messageEntity = new Message
            {
                Id = Guid.NewGuid(),
                Content = message,
                ConversationId = conversationId,
                CreateAt = DateTime.Now,
                CreateBy = WebSecurity.CurrentUserId
            };

            _messageRepository.Save(messageEntity);

            Clients.Users(usersNames).addPrivateMessage(conversationId, message);
        }
    }
}
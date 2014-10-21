using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using LiveChat.Domain.Infrastructure.Interfaces;
using LiveChat.Domain.Models.EntityClasses;
using Microsoft.AspNet.SignalR;
using System;
using System.Linq;
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

        public void Send(Guid conversationId, string message, string url)
        {
            var userIds = _usersInConversationRepository.GetUsersIdsForConversation(conversationId).ToList();
            var usersNames = _usersRepository.GetAll().Where(x => userIds.Contains(x.UserId)).ToList().Select(x => x.UserName).ToList();

            var messageEntity = new Message
            {
                Content = message,
                ConversationId = conversationId,
                CreateBy = WebSecurity.CurrentUserId
            };
            _messageRepository.Save(messageEntity);

            var author = _usersRepository.GetById(messageEntity.CreateBy);
            Clients.Users(usersNames).addPrivateMessage(conversationId, author.Name, message);

            Clients.Users(usersNames).newMessageAlert(author.Name, url);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using LiveChat.Domain.Common.Helpers;
using LiveChat.Domain.Infrastructure.Interfaces;
using LiveChat.Domain.Models.EntityExtensions;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using WebMatrix.WebData;

namespace LiveChat.App.Hubs
{
    public class UserHandlerHub : Hub
    {
        private readonly IUsersInConversationsRepository _usersInConversationRepository;
        private readonly IUserRepository _usersRepository;

        public UserHandlerHub(IUsersInConversationsRepository conversationRepository, IUserRepository usersRepository)
        {
            _usersInConversationRepository = conversationRepository;
            _usersRepository = usersRepository;
        }

        public void SendInvitation(Guid conversationId, string url)
        {
            var userIds = _usersInConversationRepository.GetUsersIdsForConversation(conversationId).ToList();
            userIds.Remove(WebSecurity.CurrentUserId);
            var usersNames = _usersRepository.GetAll().Where(x => userIds.Contains(x.UserId)).ToList().Select(x => x.UserName).ToList();
            var author = _usersRepository.GetById(WebSecurity.CurrentUserId);

            Clients.Users(usersNames).newMessageAlert(author.Name, url);
        }

        public override Task OnConnected()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                var user = UserHandler.ConnectedUsers.SingleOrDefault(x => x.UserId == WebSecurity.CurrentUserId);

                if (user != null)
                {
                    UserHandler.ConnectedUsers.Single(x => x.UserId == WebSecurity.CurrentUserId)
                        .ConnectionIds.Add(Context.ConnectionId);
                }
                else
                {
                    UserHandler.ConnectedUsers.Add(new ConnectedUser
                    {
                        UserId = WebSecurity.CurrentUserId,
                        UserName = Context.User.Identity.Name,
                        ConnectionIds = new List<string> { Context.ConnectionId }
                    });

                    Clients.All.setConnectionStatus(WebSecurity.CurrentUserId, true);
                }
            }
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                var userId = UserHandler.ConnectedUsers.First(x => x.ConnectionIds.Contains(Context.ConnectionId)).UserId;
                UserHandler.RemoveConnectionId(userId, Context.ConnectionId);

                if (UserHandler.AreAllUsersConnectionsClosed(userId))
                {
                    UserHandler.ConnectedUsers.RemoveWhere(x => x.UserId == userId);

                    Clients.All.setConnectionStatus(userId, false);
                }
            }
            return base.OnDisconnected(stopCalled);
        }
    }
}
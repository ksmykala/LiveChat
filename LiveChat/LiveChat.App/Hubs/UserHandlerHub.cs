using System.Collections.Generic;
using System.Linq;
using LiveChat.Domain.Common.Helpers;
using LiveChat.Domain.Models.EntityExtensions;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using WebMatrix.WebData;

namespace LiveChat.App.Hubs
{
    public class UserHandlerHub : Hub
    {
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

                    var message = string.Format("{0} has joined", Context.User.Identity.Name);
                    Clients.AllExcept(Context.ConnectionId).addInformation(message);
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

                    var message = string.Format("{0} has left", Context.User.Identity.Name);
                    Clients.AllExcept(Context.ConnectionId).addInformation(message);
                }
            }
            return base.OnDisconnected(stopCalled);
        }
    }
}
using System.Linq;
using LiveChat.Domain.Common.Helpers;
using LiveChat.Domain.Models.EntityClasses;
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
                UserHandler.ConnectedUsers.Add(new ConnectedUser
                {
                    //UserId = WebSecurity.CurrentUserId,
                    ConnectionIds = Context.ConnectionId,
                    UserName = Context.User.Identity.Name
                });

                var message = string.Format("{0} has joined", Context.User.Identity.Name);
                Clients.AllExcept(Context.ConnectionId).addInformation(message);
            }
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                UserHandler.ConnectedUsers.RemoveWhere(x => x.ConnectionIds == Context.ConnectionId);

                var message = string.Format("{0} has left", Context.User.Identity.Name);
                Clients.AllExcept(Context.ConnectionId).addInformation(message);
            }
            return base.OnDisconnected(stopCalled);
        }
    }
}
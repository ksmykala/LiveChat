using LiveChat.Domain.Infrastructure.Concrete;
using LiveChat.Domain.Models.EntityClasses;
using Microsoft.AspNet.SignalR;
using WebMatrix.WebData;

namespace LiveChat.App.Hubs
{
    public class ShoutboxHub : Hub
    {
        public void Send(string message)
        {
            using(var dbContext = new LiveChatContext())
            {
                var repo = new MessageRepository(dbContext);
                repo.Save(new Message { Content = message, CreateBy = WebSecurity.CurrentUserId });
            }

            Clients.All.addNewShoutboxMessage(message);
        }
    }
}
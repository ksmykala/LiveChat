using Microsoft.AspNet.SignalR;

namespace LiveChatApp.Hubs
{
    public class ShoutboxHub : Hub
    {
        public void Send(string message)
        {
            Clients.All.addNewMessage(message);
        }
    }
}
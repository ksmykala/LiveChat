using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace LiveChat.App.Hubs
{
    public class PrivateChatHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
    }
}
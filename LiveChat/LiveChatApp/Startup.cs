using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(LiveChatApp.Startup))]
namespace LiveChatApp
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
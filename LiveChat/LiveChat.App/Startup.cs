using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(LiveChat.App.Startup))]
namespace LiveChat.App
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
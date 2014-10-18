using System.Web.Mvc;

namespace LiveChatApp.Controllers
{
    public class ChatController : Controller
    {
        public ActionResult Shoutbox()
        {
            return View();
        }
    }
}

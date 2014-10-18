using System.Web.Mvc;

namespace LiveChatApp.Controllers
{
    public class ChatController : Controller
    {
        [Authorize]
        public ActionResult Shoutbox()
        {
            return View();
        }
    }
}

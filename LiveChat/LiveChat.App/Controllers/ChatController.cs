using LiveChat.Domain.Infrastructure.Concrete;
using LiveChat.Domain.Models.EntityClasses;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace LiveChat.App.Controllers
{
    public class ChatController : Controller
    {
        public ActionResult Shoutbox()
        {
            IList<Message> result;
            using (var dbContext = new LiveChatContext())
            {
                result = new MessageRepository(dbContext).GetLastMessages(10).ToList();
            }

            return View(result);
        }

    }
}

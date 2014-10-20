using LiveChat.Domain.Infrastructure.Interfaces;
using LiveChat.Domain.Models.EntityClasses;
using System;
using System.Linq;
using System.Web.Mvc;

namespace LiveChat.App.Controllers
{
    public class ChatController : Controller
    {
        private readonly IRepository<Message> _repository;

        public ChatController(IRepository<Message> repository)
        {
            _repository = repository;
        }

        [Authorize]
        public ActionResult Shoutbox()
        {
            var result = _repository.GetAll().Where(x => x.ConversationId == null).Take(10).ToList();

            return View(result);
        }

    }
}

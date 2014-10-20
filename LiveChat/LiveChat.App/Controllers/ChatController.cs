using System.Collections.Generic;
using LiveChat.Domain.Infrastructure.Interfaces;
using LiveChat.Domain.Infrastructure.Repositories;
using LiveChat.Domain.Models.EntityClasses;
using System.Linq;
using System.Web.Mvc;
using LiveChat.Domain.Models.EntityExtensions;
using WebMatrix.WebData;

namespace LiveChat.App.Controllers
{
    [Authorize(Roles = "Administrator, User")]
    public class ChatController : Controller
    {
        private readonly IRepository<Message> _messageRepository;
        private readonly IConversationRepository _conversationRepository;
        private readonly IUserRepository _userRepository;

        public ChatController(IRepository<Message> repository, 
            IConversationRepository conversationRepository,
            IUserRepository userRepository)
        {
            _messageRepository = repository;
            _userRepository = userRepository;
            _conversationRepository = conversationRepository;
        }

        public ViewResult Shoutbox()
        {
            var result = _messageRepository.GetAll()
                .Where(x => x.ConversationId == null)
                .Take(10)
                .ToList();

            return View(result);
        }

        public ViewResult UsersListChat()
        {
            var result = _userRepository.GetAll()
                .Select(x => new ChatUserViewModel
                {
                    UserId = x.UserId,
                    Nickname = x.Nickname,
                    UserName = x.UserName
                })
                .ToList();
            return View(result);
        }

        public ActionResult PrivateChat(int? userId)
        {
            var usersEntities = _userRepository.GetAll()
                .Where(x => x.UserId == userId.Value)
                .ToList();

            if (usersEntities.Any())
            {
                var currentUser = _userRepository.GetById(WebSecurity.CurrentUserId);
                usersEntities.Add(currentUser);
                var conversation = _conversationRepository.GetConversationForUsers(usersEntities);

                var model = new PrivateChatViewModel
                {
                    Users = usersEntities.Select(x => new ChatUserViewModel
                    {
                        UserId = x.UserId,
                        Nickname = x.Nickname,
                        UserName = x.UserName
                    })
                        .ToList(),
                    Last10Messages = _messageRepository.GetAll()
                        .Where(x => x.ConversationId == conversation.Id)
                        .OrderByDescending(x => x.CreateAt)
                };
                return View(model);
            }

            ModelState.AddModelError(string.Empty, "You have to choose user.");
            return View("UsersListChat");
        }
    }
}

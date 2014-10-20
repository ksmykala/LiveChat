using LiveChat.Domain.Infrastructure.Interfaces;
using LiveChat.Domain.Models.EntityClasses;
using LiveChat.Domain.Models.EntityExtensions;
using System.Linq;
using System.Web.Mvc;
using LiveChat.Domain.ViewModels;
using WebMatrix.WebData;

namespace LiveChat.App.Controllers
{
    [Authorize(Roles = "Administrator, User")]
    public class ChatController : Controller
    {
        private readonly IRepository<Message> _messageRepository;
        private readonly IConversationRepository _conversationRepository;
        private readonly IUsersInConversationsRepository _usersInConversationsRepository;
        private readonly IUserRepository _userRepository;

        public ChatController(IRepository<Message> repository, 
            IConversationRepository conversationRepository,
            IUserRepository userRepository, 
            IUsersInConversationsRepository usersInConversationsRepository)
        {
            _messageRepository = repository;
            _userRepository = userRepository;
            _usersInConversationsRepository = usersInConversationsRepository;
            _conversationRepository = conversationRepository;
        }

        public ViewResult Shoutbox()
        {
            var lastMessages = _messageRepository.GetAll()
                .Where(x => x.ConversationId == null)
                .OrderBy(x => x.CreateAt)
                .Take(50)
                .ToList();

            var result = lastMessages.Select(x => new UserMessage
            {
                Content = x.Content,
                AuthorId = x.CreateBy,
                CreatedAt = x.CreateAt,
                Author = new ChatUserViewModel(_userRepository.GetById(x.CreateBy))
            });

            return View(result);
        }

        public ViewResult UsersListChat()
        {
            var result = _userRepository.GetAll()
                .Where(x => x.UserId != WebSecurity.CurrentUserId)
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
                var conversationId = _usersInConversationsRepository.GetConversationForUsers(usersEntities);

                var chatUsers = usersEntities.Select(x => new ChatUserViewModel
                {
                    UserId = x.UserId,
                    Nickname = x.Nickname,
                    UserName = x.UserName
                })
                .ToList();

                var model = new PrivateChatViewModel
                {
                    ConversationId = conversationId,
                    Users = chatUsers,
                    Messages = _messageRepository.GetAll()
                        .Where(x => x.ConversationId == conversationId)
                        .OrderByDescending(x => x.CreateAt)
                        .ToList()
                        .Select(x => new UserMessage
                        {
                            AuthorId = x.CreateBy,
                            Content = x.Content,
                            Author = chatUsers.SingleOrDefault(y => y.UserId == x.CreateBy),
                            CreatedAt = x.CreateAt
                        })
                        .ToList()
                };
                return View(model);
            }

            ModelState.AddModelError(string.Empty, "You have to choose user.");
            return View("UsersListChat");
        }
    }
}

using LiveChat.Domain.Infrastructure.Interfaces;
using LiveChat.Domain.Models.EntityClasses;
using Microsoft.AspNet.SignalR;
using WebMatrix.WebData;

namespace LiveChat.App.Hubs
{
    public class ShoutboxHub : Hub
    {
        private readonly IRepository<Message> _repository;
        private readonly IUserRepository _userRepository;

        public ShoutboxHub(IRepository<Message> repository, IUserRepository userRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
        }

        public void Send(string message)
        {
            _repository.Save(new Message { Content = message, CreateBy = WebSecurity.CurrentUserId });
            var author = _userRepository.GetById(WebSecurity.CurrentUserId);

            Clients.All.addNewShoutboxMessage(author.Name, message);
        }
    }
}
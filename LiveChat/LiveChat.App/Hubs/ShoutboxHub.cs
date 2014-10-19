using LiveChat.Domain.Infrastructure.Interfaces;
using LiveChat.Domain.Models.EntityClasses;
using Microsoft.AspNet.SignalR;
using WebMatrix.WebData;

namespace LiveChat.App.Hubs
{
    public class ShoutboxHub : Hub
    {
        private readonly IRepository<Message> _repository;

        public ShoutboxHub(IRepository<Message> repository)
        {
            _repository = repository;
        }

        public void Send(string message)
        {
            _repository.Save(new Message{Content = message, CreateBy = WebSecurity.CurrentUserId});

            Clients.All.addNewShoutboxMessage(message);
        }
    }
}
using LiveChat.Domain.Infrastructure.Interfaces;
using LiveChat.Domain.Infrastructure.Repositories;
using LiveChat.Domain.Models.EntityClasses;
using Microsoft.AspNet.SignalR;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LiveChat.App.Infrastructure.Ninject
{
    public class SignalRNinjectDependencyResolver : DefaultDependencyResolver
    {
        private readonly IKernel _ninjectKernel;

        public SignalRNinjectDependencyResolver()
        {
            _ninjectKernel = new StandardKernel();
            AddBindings();
        }

        public override object GetService(Type serviceType)
        {
            return _ninjectKernel.TryGet(serviceType) ?? base.GetService(serviceType);
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            return _ninjectKernel.GetAll(serviceType).Concat(base.GetServices(serviceType));
        }

        private void AddBindings()
        {
            _ninjectKernel.Bind<IRepository<Message>>().To<MessageRepository>();
            _ninjectKernel.Bind<IConversationRepository>().To<ConversationRepository>();
            _ninjectKernel.Bind<IUserRepository>().To<UserRepository>();
        }
    }
}
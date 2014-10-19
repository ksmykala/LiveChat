using System;
using System.Web.Mvc;
using LiveChat.Domain.Infrastructure.Interfaces;
using LiveChat.Domain.Infrastructure.Repositories;
using LiveChat.Domain.Models.EntityClasses;
using Ninject;

namespace LiveChat.App.Infrastructure.Ninject
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private readonly IKernel _ninjectKernel;

        public NinjectControllerFactory()
        {
            _ninjectKernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController)_ninjectKernel.Get(controllerType);
        }

        private void AddBindings()
        {
            _ninjectKernel.Bind<IRepository<Message>>().To<MessageRepository>();
            _ninjectKernel.Bind<IRepository<User>>().To<UserRepository>();
        }
    }
}
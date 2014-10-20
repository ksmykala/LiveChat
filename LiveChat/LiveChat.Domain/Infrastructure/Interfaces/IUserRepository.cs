using LiveChat.Domain.Models.EntityClasses;
using System.Linq;

namespace LiveChat.Domain.Infrastructure.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User GetById(int id);
        IQueryable<User> GetById(int[] ids);
    }
}

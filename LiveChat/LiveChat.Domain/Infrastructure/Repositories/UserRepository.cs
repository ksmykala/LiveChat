using LiveChat.Domain.Infrastructure.Interfaces;
using LiveChat.Domain.Models.EntityClasses;
using System.Linq;

namespace LiveChat.Domain.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public User GetById(int id)
        {
            var result = GetAll().SingleOrDefault(x => x.UserId == id);
            return result;
        }

        public IQueryable<User> GetById(int[] ids)
        {
            var result = GetAll().Where(x => ids.Contains(x.UserId));
            return result;
        }
    }
}

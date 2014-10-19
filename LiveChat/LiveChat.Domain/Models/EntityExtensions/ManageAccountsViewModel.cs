using LiveChat.Domain.Models.EntityClasses;
using System.Collections.Generic;

namespace LiveChat.Domain.Models.EntityExtensions
{
    public class ManageAccountsViewModel
    {
        public IEnumerable<User> Users { get; set; }
        public RegisterModel RegisterModel { get; set; }
    }
}

using System.Collections.Generic;
using LiveChat.Domain.Models.EntityClasses;

namespace LiveChat.Domain.ViewModels
{
    public class ManageAccountsViewModel
    {
        public IEnumerable<User> Users { get; set; }
        public RegisterModel RegisterModel { get; set; }
    }
}


namespace LiveChat.Domain.Models.EntityClasses
{
    public partial class User
    {
        public int UserRolesCount { get; set; }

        public string Name
        {
            get { return string.IsNullOrEmpty(Nickname) ? UserName : Nickname; }
        }
    }
}

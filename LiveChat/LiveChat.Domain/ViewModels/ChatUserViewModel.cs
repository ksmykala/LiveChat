
using LiveChat.Domain.Models.EntityClasses;

namespace LiveChat.Domain.ViewModels
{
    public class ChatUserViewModel
    {
        public ChatUserViewModel()
        {
        }

        public ChatUserViewModel(User userEntity)
        {
            if (userEntity != null)
            {
                UserId = userEntity.UserId;
                UserName = userEntity.UserName;
                Nickname = userEntity.Nickname;
            }
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Nickname { get; set; }
        public bool IsConnected { get; set; }

        public string Name
        {
            get { return string.IsNullOrEmpty(Nickname) ? UserName : Nickname; }
        }
    }
}

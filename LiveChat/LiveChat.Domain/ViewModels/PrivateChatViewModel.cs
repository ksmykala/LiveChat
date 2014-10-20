using LiveChat.Domain.Models.EntityExtensions;
using System;
using System.Collections.Generic;

namespace LiveChat.Domain.ViewModels
{
    public class PrivateChatViewModel
    {
        public Guid ConversationId { get; set; }
        public IEnumerable<ChatUserViewModel> Users { get; set; }
        public IEnumerable<ChatUserViewModel> ConnectedUserIds { get; set; }
        public IEnumerable<UserMessage> Messages { get; set; }
    }
}

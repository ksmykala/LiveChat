using System;
using System.Collections.Generic;
using LiveChat.Domain.Models.EntityClasses;
using LiveChat.Domain.Models.EntityExtensions;

namespace LiveChat.Domain.ViewModels
{
    public class PrivateChatViewModel
    {
        public Guid ConversationId { get; set; }
        public IEnumerable<ChatUserViewModel> Users { get; set; }
        public IEnumerable<ChatUserViewModel> ConnectedUserIds { get; set; }
        public IEnumerable<Message> Last10Messages { get; set; }
    }
}

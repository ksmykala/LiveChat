using System;
using LiveChat.Domain.Models.EntityClasses;
using System.Collections.Generic;

namespace LiveChat.Domain.Models.EntityExtensions
{
    public class PrivateChatViewModel
    {
        public Guid ConversationId { get; set; }
        public IEnumerable<ChatUserViewModel> Users { get; set; }
        public IEnumerable<ChatUserViewModel> ConnectedUserIds { get; set; }
        public IEnumerable<Message> Last10Messages { get; set; }
    }
}

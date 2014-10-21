using System;
using LiveChat.Domain.ViewModels;

namespace LiveChat.Domain.Models.EntityExtensions
{
    public class UserMessage
    {
        public int AuthorId { get; set; }
        public ChatUserViewModel Author { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

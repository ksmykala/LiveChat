using System;
using System.Collections.Generic;

namespace LiveChat.Domain.Models.EntityExtensions
{
    public class ConnectedUser
    {
        //public int UserId { get; set; }
        public string ConnectionIds { get; set; }
        public string UserName { get; set; }
    }
}

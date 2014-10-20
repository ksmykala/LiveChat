using LiveChat.Domain.Models.EntityExtensions;
using System.Collections.Generic;

namespace LiveChat.Domain.Common.Helpers
{
    public static class UserHandler
    {
        public static HashSet<ConnectedUser> ConnectedUsers = new HashSet<ConnectedUser>();
    }
}

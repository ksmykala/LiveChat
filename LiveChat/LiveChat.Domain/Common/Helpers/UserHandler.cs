using System.Linq;
using LiveChat.Domain.Models.EntityExtensions;
using System.Collections.Generic;

namespace LiveChat.Domain.Common.Helpers
{
    public static class UserHandler
    {
        public static HashSet<ConnectedUser> ConnectedUsers = new HashSet<ConnectedUser>();

        public static void RemoveConnectionId(int userId, string connectionId)
        {
            ConnectedUsers.Single(x => x.UserId == userId).ConnectionIds.Remove(connectionId);
        }

        public static bool AreAllUsersConnectionsClosed(int userId)
        {
            var result = !ConnectedUsers.Single(x => x.UserId == userId).ConnectionIds.Any();
            return result;
        }

        public static bool IsUserConnected(int userId)
        {
            var result = ConnectedUsers.Select(x => x.UserId).Contains(userId);
            return result;
        }
    }
}

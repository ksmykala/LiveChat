using LiveChat.Domain.Models.EntityClasses;

namespace LiveChat.Domain.Models.EntityExtensions
{
    public class LiveChatContextSingleton
    {
        private static LiveChatContext _instance;

        private LiveChatContextSingleton()
        {
        }

        public static LiveChatContext Instance
        {
            get { return _instance ?? (_instance = new LiveChatContext()); }
        }
    }
}

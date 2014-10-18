using WebMatrix.WebData;

namespace LiveChat.App
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "Users", "UserId", "UserName", autoCreateTables: true);
        }
    }
}

namespace SyncChat.Services
{
    public static class UserConnectionService
    {
        private static Dictionary<string, string> _connections = new Dictionary<string, string>();

        public static void AddConnection(string user, string connectionId) 
        {
            RemoveConnection(user);
            _connections.Add(user, connectionId);
        }

        public static void RemoveConnection(string user) 
        {
            _connections.Remove(user);
        }

        public static string GetUserConnection(string user) 
        {
            string connId = string.Empty;

            if(_connections.TryGetValue(user, out string? Id))
                connId = Id;

            return connId;
        }
    }
}

namespace SyncChat.Models.Results
{
    public class DashboardResult
    {
        public User User { get; set; }
        public IList<Chat> AllChats { get; set; } = new List<Chat>();

        public Chat? SelectedChat { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace SyncChat.Models
{
    public class Chat
    {
        [Key]
        public int ChatId { get; set; }
        public string ChatName { get; set; }
        public DateTime CreatedAt { get; set; }

        //Relations
        public ICollection<User> Users { get; } = new List<User>();
        public ICollection<Message> Messages { get; } = new List<Message>();
    }
}

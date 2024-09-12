using System.ComponentModel.DataAnnotations.Schema;

namespace SyncChat.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }

        //Relations
        public User Owner { get; set; }

        [ForeignKey("ChatId")]
        public Chat Chat { get; set; }

        public int ChatId { get; set; }

    }
}

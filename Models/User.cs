using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyncChat.Models
{
    public class User : IdentityUser<Guid>
    {
        public string DisplayName { get; set; }

        public ICollection<Chat> Chats { get; } = new List<Chat>();

        public FileUpload ProfilePicture { get; set; } = null!;

        [NotMapped]
        public IFormFile ProfilePictureUpload { get; set; } = null!;

        [NotMapped]
        public Chat SelectedChat { get; set; }
    }
}

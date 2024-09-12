using System.ComponentModel.DataAnnotations;

namespace SyncChat.Models
{
    public class FileUpload
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Path { get; set; }
    }
}

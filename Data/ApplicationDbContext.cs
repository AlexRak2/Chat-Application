using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SyncChat.Models;

namespace SyncChat.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                //chats
                entity.HasKey(u => u.Id);

                entity.HasMany(u => u.Chats)
                .WithMany(c => c.Users);
            });

            modelBuilder.Entity<Chat>(entity =>
            {
                //users
                //messages
                entity.HasKey(c => c.ChatId);

                entity.HasMany(c => c.Users)
                .WithMany(u => u.Chats);

                entity.HasMany(c => c.Messages)
                .WithOne(m => m.Chat)
                .HasForeignKey(m => m.ChatId);
            });

            modelBuilder.Entity<Message>(entity =>
            {
                //users
                //messages
                entity.HasKey(m => m.MessageId);

                entity.HasOne(m => m.Chat)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ChatId);
            });
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<FileUpload> FileUpload { get; set; }

    }
}

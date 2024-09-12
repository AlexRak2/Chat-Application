using Microsoft.EntityFrameworkCore;
using SyncChat.Data;
using SyncChat.Hubs;
using SyncChat.Models;

namespace SyncChat.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserService> _logger;
        public UserService(ApplicationDbContext dataContext, ILogger<UserService> logger)
        {
            _context = dataContext;
            _logger = logger;
        }


        public async Task<User> GetUserByEmailAsync(string email)
        {
            User? user = await _context.Users.Include(x => x.Chats)
                .ThenInclude(x => x.Users)
                .Include(x => x.Chats)
                .ThenInclude(x => x.Messages)
                .FirstOrDefaultAsync(x => x.Email == email);

            return user;
        }
    }
}

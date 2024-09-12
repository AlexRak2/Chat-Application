using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using SyncChat.Data;
using SyncChat.Models;
using SyncChat.Services;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SyncChat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ChatHub> _logger;
        private readonly UserService _userService;
        public ChatHub(ApplicationDbContext dataContext, ILogger<ChatHub> logger, UserService userService)
        {
            _context = dataContext;
            _logger = logger;
            _userService = userService;
        }

        public async Task sendMessage(string message)
        {
            User? user = await _context.Users.Include(x => x.Chats)
                .ThenInclude(x => x.Users)
                .Include(x => x.Chats)
                .ThenInclude(x => x.Messages)
                .Include(x => x.ProfilePicture)
                .FirstOrDefaultAsync(x => x.Email == Context.User.Identity.Name);

            Message newMessage = new Message();
            newMessage.SentAt = DateTime.Now;
            newMessage.Content = message;
            newMessage.Owner = user;
            newMessage.ChatId = user.Chats.First().ChatId;

            Chat chat = user.Chats.First();
            chat.Messages.Add(newMessage);

            await _context.Messages.AddAsync(newMessage);
            await _context.SaveChangesAsync();

            await Clients.Group(chat.ChatId.ToString()).SendAsync("receiveMessage", user.DisplayName, message, DateTime.Now.ToString("M/d/yyyy h:mm:ss tt"), user.ProfilePicture.Path);
        }

        public async Task addToGroup(string groupId) 
        {
            _logger.LogWarning($"test {groupId.Trim()}");

            await Groups.AddToGroupAsync(Context.ConnectionId, groupId.Trim());
        }

        public override Task OnConnectedAsync()
        {
            UserConnectionService.AddConnection(Context.User.Identity.Name, Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            UserConnectionService.RemoveConnection(Context.User.Identity.Name);

            return base.OnDisconnectedAsync(exception);
        }
    }
}

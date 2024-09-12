using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SyncChat.Data;
using SyncChat.Hubs;
using SyncChat.Models;
using SyncChat.Models.Results;
using SyncChat.Services;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace SyncChat.Controllers
{
    public class ChatController : Controller
    {
        private readonly ILogger<ChatController> _logger;

        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly UserService _userService;
        private readonly IHubContext<ChatHub> _chatHub;


        public ChatController(ILogger<ChatController> logger, ApplicationDbContext dataContext, UserManager<User> userManager, UserService userService, IHubContext<ChatHub> chatHub)
        {
            _logger = logger;
            _context = dataContext;
            _userManager = userManager;
            _userService = userService;
            _chatHub = chatHub;
        }

        [Authorize]
        public async Task<IActionResult> Index(int? chatId)
        {
            User? user = await _userService.GetUserByEmailAsync(User.Identity.Name);
            if (user == null)
                return View();

            ViewBag.Username = user.DisplayName;

            Chat? selectedChat = null;
            if (chatId != null)
            {
                selectedChat = await _context.Chats.Include(x => x.Users).Include(x => x.Messages).ThenInclude(x => x.Owner).ThenInclude(x => x.ProfilePicture).FirstOrDefaultAsync(c => c.ChatId == chatId);

                if (selectedChat != null)
                {
                    var previousChat = user.Chats.Count > 0 ? user.Chats.First() : null;



                    if (previousChat != null) 
                    {
                        _context.Chats.Update(previousChat);
                        previousChat.Users.Remove(user);
                    }

                    _context.Chats.Update(selectedChat);
                    selectedChat.Users.Add(user);
                }
      

                await _context.SaveChangesAsync();
            }

            ViewBag.CurrentName = user.DisplayName;

            if (selectedChat != null) 
            {             
               var newChat = await _context.Chats.Include(x => x.Users).Include(x => x.Messages).ThenInclude(x => x.Owner).FirstOrDefaultAsync(c => c.ChatId == chatId);
            }


            DashboardResult dashboardResult = new DashboardResult() 
            {
                User = user,
                AllChats = _context.Chats.Include(x => x.Users).ThenInclude(x => x.ProfilePicture).ToList<Chat>(),
                SelectedChat = selectedChat
            };

            if (user != null)
                return View(dashboardResult);

            return View();

        }

        public async Task<IActionResult> CreateChat(string name)
        {
            var chat = new Chat()
            {
                ChatName = name,
                CreatedAt = DateTime.Now
            };

            await _context.Chats.AddAsync(chat);
            await _context.SaveChangesAsync();


            return RedirectToAction("Index");
        }

    }
}

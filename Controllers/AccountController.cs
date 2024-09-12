using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyncChat.Models.Results;
using SyncChat.Models;
using SyncChat.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using SyncChat.Data;
using SyncChat.Hubs;
using Microsoft.EntityFrameworkCore;
using MeChat.Services;

namespace SyncChat.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;

        private readonly ApplicationDbContext _context;
        private readonly UserService _userService;
        private readonly FileUploaderService _fileUploaderService;



        public AccountController(ILogger<AccountController> logger, ApplicationDbContext dataContext, UserService userService , FileUploaderService fileUploaderService)
        {
            _logger = logger;
            _context = dataContext;
            _userService = userService;
            _fileUploaderService = fileUploaderService;
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {

            User? user = await _context.Users.Include(x => x.ProfilePicture).FirstOrDefaultAsync(x => x.Email == User.Identity.Name);

            if (user == null)
                return RedirectToAction("Index");

            Profile profile = new Profile();
            profile.User = user;

            return View(profile);
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Save(Profile profile)
        {

            User? user = await _userService.GetUserByEmailAsync(User.Identity.Name);
            if (user == null)
                return RedirectToAction("Index");

            _context.Update(user);


            if (profile.User.ProfilePictureUpload != null)
            {
                _fileUploaderService.GetFilePath(user, profile.User.ProfilePictureUpload, out string databaseFilePath);
                if (_fileUploaderService.GetFileExists(databaseFilePath, out FileUpload? fileUpload))
                {
                    user.ProfilePicture = fileUpload!;
                }
                else
                {
                    FileUpload newFileUpload = await _fileUploaderService.UploadFile(user, profile.User.ProfilePictureUpload);
                    user.ProfilePicture = newFileUpload;
                }
            }
            

            user.DisplayName = profile.User.DisplayName;

            await _context.SaveChangesAsync();

            return RedirectToAction("Profile");
        }

    }
}

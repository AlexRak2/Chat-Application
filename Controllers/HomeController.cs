using Microsoft.AspNetCore.Mvc;
using SyncChat.Data;
using SyncChat.Models;
using SyncChat.Models.Results;
using System.Diagnostics;

namespace SyncChat.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated) 
            {
                return RedirectToAction("Index", "Chat", "");
            }

            Statistics statistics = new Statistics();

            statistics.TotalUsers = _context.Users.Count();
            statistics.TotalMessages = _context.Messages.Count();

            return View(statistics);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

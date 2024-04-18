using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using WebApplication2.Models;
using Mail.Domain.Entities;
using Mail.Domain;
using Mail.MVC.Models.Home;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Mail.MVC.Models.Message;
namespace WebApplication2.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _context;

        private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        private User CurrentUser => _context.Users.Where(u => u.Id ==  CurrentUserId).FirstOrDefault();

        public HomeController(ILogger<HomeController> logger, ApplicationContext ctx)
        {
            _context = ctx;
            _logger = logger;
        }

        public IActionResult Index(string error = null)
        {
            if (!string.IsNullOrEmpty(error))
            {
                ViewBag.Error = error;
            }
            var msgs = _context.Messages
                .Where(m => m.ReceiverId == CurrentUserId)
                .Join(_context.Users, m => m.ReceiverId, u => u.Id, 
                                       (m,u) => new MessageViewModel { Id   = m.Id,
                                                                       Body = m.Body,
                                                                       Date = m.Date,
                                                                       Title = m.Title,
                                                                       Sender = u.Login,
                                                                       IsRead = m.IsRead})
                .OrderByDescending((t => t.Date))
                .ToList();
            return View(new HomeViewModel
            {
                User = CurrentUser,
                Messages = msgs
            });
        }
       
        [HttpPost]
        public IActionResult FilterMessages(string senderLogin, DateTime? startDate, DateTime? endDate, string status)
        {
            var query = _context.Messages.Where(m => m.ReceiverId == CurrentUserId)
                .Join(_context.Users, m => m.ReceiverId, u => u.Id,
                                       (m, u) => new MessageViewModel
                                       {
                                           Id = m.Id,
                                           Body = m.Body,
                                           Date = m.Date,
                                           Title = m.Title,
                                           Sender = u.Login,
                                           IsRead = m.IsRead
                                       });

            // Применяем фильтр по логину отправителя, если указан
            if (!string.IsNullOrEmpty(senderLogin))
            {
                query = query.Where(m => m.Sender == senderLogin);
            }

            // Применяем фильтр по интервалу дат отправки, если указаны даты начала и/или окончания
            if (startDate != null)
            {
                query = query.Where(m => m.Date >= startDate.Value.ToUniversalTime());
            }
            if (endDate != null)
            {
                query = query.Where(m => m.Date <= endDate.Value.ToUniversalTime());
            }

            // Применяем фильтр по статусу (непрочитанные или все)
            if (status == "unread")
            {
                query = query.Where(m => !m.IsRead);
            }

            var filteredMessages = query.ToList();

            return View("Index", new HomeViewModel
            {
                User = CurrentUser,
                Messages = filteredMessages
            });
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

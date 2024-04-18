using Mail.Domain;
using Mail.Domain.Entities;
using Mail.MVC.Models.Message;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Mail.MVC.Controllers
{
    public class MessageController : Controller
    {
        private readonly ApplicationContext _context;

        public MessageController(ApplicationContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendMessageAsync(SendMessageModel model)
        {
            var reciever = _context.Users.Where(u => u.Login == model.Reciever).FirstOrDefault();
            if (reciever == null)
            {
                ViewBag.Error = "Пользователя с таким логином не существует";
                return RedirectToAction("Index", "Home", new { error = ViewBag.Error });
            }
            Message message = new Message {
                Title = model.Title,
                Body = model.Body, 
                ReceiverId = reciever.Id,
                IsRead = false,
                Date = DateTime.UtcNow,
                SenderId = model.SenderId
            };

            await _context.AddAsync(message);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> ReadMessage(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            message.IsRead = true;

            _context.Messages.Update(message);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}

using Mail.Domain;
using Mail.Domain.Entities;
using Mail.MVC.Models.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Mail.MVC.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationContext _context;
        public AccountController(ApplicationContext ctx) {
            _context = ctx;
        }
        public IActionResult Index()
        {
            return View();
        }

        private async Task AuthenticateAsync(User user)
        {
            var claims = new List<Claim> { 
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
            };

            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> LoginAsync(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => (u.Login ==  model.Login) && (u.Password == model.Password) );
            if (user is null)
            {
                ViewBag.Error = "Некорректные логин или пароль";
                return View("Index", model);
            }

            await AuthenticateAsync(user);
            return RedirectToAction("Index","Home");
        }


        public async Task<IActionResult> RegisterAsync(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine(model.Login);
                Console.WriteLine("ModelState is not valid. Errors:");
                foreach (var modelStateEntry in ModelState.Values)
                {
                    foreach (var error in modelStateEntry.Errors)
                    {
                        Console.WriteLine($"- {error.ErrorMessage}");
                    }
                }
                return View("Register", model);
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => (u.Login == model.Login) && (u.Password == model.Password));
            if (user != null)
            {
                ViewBag.Error = "Такой пользователь уже существует";
                return View("Register", model);
            }   

            user = new User { Login = model.Login, Name = model.Name, Password = model.Password };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            await AuthenticateAsync(user);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Account");
        }


    }
}

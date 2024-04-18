using System.ComponentModel.DataAnnotations;

namespace Mail.MVC.Models.Account
{
    public class AccountViewModel
    {
        public LoginViewModel LoginViewModel { get; set; }
        public RegisterViewModel RegisterViewModel { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Поле обязательно")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Поле обязательно")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Поле обязательно")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Поле обязательно")]

        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string PasswordRepeat { get; set; }

    }
}

using Mail.Domain.Entities;
using Mail.MVC.Models.Message;

namespace Mail.MVC.Models.Home

{
    public class HomeViewModel
    {
        public User User { get; set; }
        public List<MessageViewModel> Messages { get; set; }
    }
}

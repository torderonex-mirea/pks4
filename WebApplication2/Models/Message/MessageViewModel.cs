namespace Mail.MVC.Models.Message
{
    public class MessageViewModel
    {
    public int Id { get; set; }
        public string Sender { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public bool IsRead { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}

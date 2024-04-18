namespace Mail.MVC.Models.Message
{
    public class SendMessageModel
    {
        public string Body { get; set; }
        public string Title { get; set; }
        public string Reciever { get; set; }

        public int SenderId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mail.Domain.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public int ReceiverId { get; set; }
        public int SenderId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public bool IsRead { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}

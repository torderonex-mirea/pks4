using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mail.Domain.Entities
{
    public class User
    {
            public int Id { get; set; }
            public required string Login { get; set; }
            public string Password { get; set; }
            public string Name { get; set; }

    }
}

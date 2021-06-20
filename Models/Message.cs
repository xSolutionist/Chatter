using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatter.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string RecievingUser { get; set; }
        public string SendingUser { get; set; }
        public string Comment { get; set; }

        public string UserImage { get; set; }

        public DateTime Created { get; set; }
    }
}

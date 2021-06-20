using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatter.Models
{
    public class Thread
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }

        public int PostId { get; set; }

    }
}

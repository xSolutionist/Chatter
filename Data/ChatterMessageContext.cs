using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Chatter.Models;

namespace Chatter.Data
{
    public class ChatterMessageContext : DbContext
    {
        public ChatterMessageContext (DbContextOptions<ChatterMessageContext> options)
            : base(options)
        {
        }

        public DbSet<Chatter.Models.Message> Message { get; set; }
    }
}

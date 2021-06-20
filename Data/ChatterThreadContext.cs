using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Chatter.Models;

namespace Chatter.Data
{
    public class ChatterThreadContext : DbContext
    {
        public ChatterThreadContext (DbContextOptions<ChatterThreadContext> options)
            : base(options)
        {
        }

        public DbSet<Chatter.Models.Thread> Thread { get; set; }
    }
}

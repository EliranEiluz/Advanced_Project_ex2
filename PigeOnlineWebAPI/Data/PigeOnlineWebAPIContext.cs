#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PigeOnlineAPI;
using PigeOnlineAPI.Controllers;

namespace PigeOnlineWebAPI.Data
{
    public class PigeOnlineWebAPIContext : DbContext
    {
        public PigeOnlineWebAPIContext (DbContextOptions<PigeOnlineWebAPIContext> options)
            : base(options)
        {
        }

        public DbSet<PigeOnlineAPI.Chat> Chat { get; set; }

        public DbSet<PigeOnlineAPI.Message> Message { get; set; }

        public DbSet<PigeOnlineAPI.Controllers.User> User { get; set; }
    }
}

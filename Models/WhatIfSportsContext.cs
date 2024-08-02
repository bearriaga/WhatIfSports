using Microsoft.EntityFrameworkCore;

namespace WhatIfSports.Models
{
    public class WhatIfSportsContext : DbContext
    {
        public DbSet<Player> Players { get; set; }

        public WhatIfSportsContext(DbContextOptions options) : base(options)
        {
            
        }
    }
}

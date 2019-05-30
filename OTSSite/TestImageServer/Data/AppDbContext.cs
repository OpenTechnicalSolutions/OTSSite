using Microsoft.EntityFrameworkCore;
using OTSSiteMVC.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestImageServer.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions dco) : base(dco)
        {

        }

        public DbSet<ImageData> Images { get; set; }
    }
}

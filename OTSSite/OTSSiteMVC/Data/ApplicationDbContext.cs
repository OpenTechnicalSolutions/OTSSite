using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OTSSiteMVC.Entities;

namespace OTSSiteMVC.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppIdentityUser, AppIdentityRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}

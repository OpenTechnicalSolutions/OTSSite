using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSiteMVC.Entities
{
    public class AppIdentityRole : IdentityRole<Guid>
    {
        public AppIdentityRole(string role) : base(role)
        {

        }

        public AppIdentityRole() : base()
        {

        }
    }
}

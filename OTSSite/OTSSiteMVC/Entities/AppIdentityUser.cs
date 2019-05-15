using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSiteMVC.Entities
{
    public class AppIdentityUser : IdentityUser<Guid>
    {
        public string Website { get; set; }
        public string Description { get; set; }
        public DateTime JoinDateTime { get; set; }

        public AppIdentityUser(string username) : base(username) { }

        public AppIdentityUser() : base() { }
    }
}

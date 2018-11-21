using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSite.ViewModel
{
    public class AddRolesViewModel
    {
        public IdentityUser User { get; set; }
        public List<IdentityRole> AvailableRoles { get; set; }
        public List<IdentityRole> AddedRoles { get; set; }
    }
}

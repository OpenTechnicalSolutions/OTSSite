using OTSSiteMVC.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSiteMVC.Models
{
    public class UserConfigDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public ImageData ProfileImage { get; set; }
        public string DelimitedAssignedRoles { get; set; }
        public string DelimitedUnAssignedRoles { get; set; }
        public bool Lockout { get; set; }
        public string[] AssignedRoles { get; set; }
        public string[] UnAssignedRoles { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSite.Models.ViewModels
{
    public class UserAdminViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string[] Roles { get; set; }
        public bool Enables { get; set; }
    }
}

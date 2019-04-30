using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSite.Models.ViewModels
{
    public class ModifyAccountViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public Dictionary<string, bool> Roles { get; set; }
        [DisplayName("Active")]
        public bool LockoutEnabled { get; set; }
        public ModifyAccountViewModel()
        {
           /* Roles = new Dictionary<string, bool>()
            {
                { "administrator", false },
                { "editor", false },
                { "author", false },
                { "user", false }
            };*/
        }
    }
}

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
        }
        /// <summary>
        /// Adds the available and currently applied roles to the user in this model.
        /// </summary>
        /// <param name="availableRoles"></param>
        /// <param name="appliedRoles"></param>
        public void AddRoles(
            IEnumerable<string> availableRoles, 
            IEnumerable<string> appliedRoles)
        {
            Roles = new Dictionary<string, bool>();
            foreach (var r in availableRoles)
                if (appliedRoles.Contains(r))
                    Roles.Add(r, true);
                else
                    Roles.Add(r, false);
        }
    }
}

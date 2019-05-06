using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using OTSSite.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSite
{
    public static class DefaultAccountSeeds
    {
        private const string DEFAULT_ACCOUNT_NAME = "Administrator";
        private const string DEFAULT_ACCOUNT_PASSWORD = "ChangeMe2019!";
        private static readonly IEnumerable<string> SYSTEM_ROLES = new string[]    
        {
            "administrator",
            "editor",
            "author",
            "user"
        };

        public static void Seed(
            UserManager<ApplicationIdentityUser> _userManager,
            RoleManager<IdentityRole> _roleManager)
        {
            
            foreach(var r in SYSTEM_ROLES)
            {
                if (_roleManager.Roles.Select(role => role.Name).Contains(r))
                    continue;
                _roleManager.CreateAsync(new IdentityRole(r)).Wait();           
            }
            var defaultAccount = new ApplicationIdentityUser(DEFAULT_ACCOUNT_NAME);

            if(!_userManager.Users.Select(u => u.UserName).Contains(DEFAULT_ACCOUNT_NAME))
                _userManager.CreateAsync(defaultAccount, DEFAULT_ACCOUNT_PASSWORD).Wait();

            _userManager.AddToRolesAsync(defaultAccount, SYSTEM_ROLES).Wait();        
        }
    }
}

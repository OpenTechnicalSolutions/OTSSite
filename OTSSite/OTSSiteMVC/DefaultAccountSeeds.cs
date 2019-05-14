using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using OTSSiteMVC.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSiteMVC
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
            UserManager<AppIdentityUser> _userManager,
            RoleManager<AppIdentityRole> _roleManager)
        {
            
            foreach(var r in SYSTEM_ROLES)
            {
                if (_roleManager.Roles.Select(role => role.Name).Contains(r))
                    continue;
                _roleManager.CreateAsync(new AppIdentityRole(r)).Wait();           
            }
            var defaultAccount = new AppIdentityUser(DEFAULT_ACCOUNT_NAME)
            {
                Email = "Administrator@local.host"
            };

            if(!_userManager.Users.Select(u => u.UserName).Contains(DEFAULT_ACCOUNT_NAME))
                _userManager.CreateAsync(defaultAccount, DEFAULT_ACCOUNT_PASSWORD).Wait();

            _userManager.AddToRolesAsync(defaultAccount, SYSTEM_ROLES).Wait();        
        }
    }
}

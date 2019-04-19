using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using OTSSite.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSite
{
    public class DefaultAccountSeeds
    {
        private const string DEFAULT_ACCOUNT_NAME = "Administrator";
        private const string DEFAULT_ACCOUNT_PASSWORD = "ChangeMe2019!";
        private readonly IEnumerable<string> SYSTEM_ROLES = new string[]    
        {
            "administrator",
            "editor",
            "author",
            "user"
        };

        private readonly UserManager<ApplicationIdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DefaultAccountSeeds(
            UserManager<ApplicationIdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> Seed()
        {
            
            foreach(var r in SYSTEM_ROLES)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(r));
                if (!result.Succeeded)               
                    throw new Exception($"Failed to create Role {r}");              
            }

            var defaultAccount = new ApplicationIdentityUser(DEFAULT_ACCOUNT_NAME);
            var res = await _userManager.CreateAsync(defaultAccount, DEFAULT_ACCOUNT_PASSWORD);
            if(!res.Succeeded)           
                throw new Exception($"Failed to create User {DEFAULT_ACCOUNT_NAME}");
            
            res = await _userManager.AddToRolesAsync(defaultAccount, SYSTEM_ROLES);
            if(!res.Succeeded)
                throw new Exception($"Failed to add {DEFAULT_ACCOUNT_NAME} to roles.");
            
            return true;
        }
    }
}

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSite
{
    public static class DefaultData
    {
        private static readonly string[] _roles =
        {
            "Administrator",
            "Moderator",
            "Author",
            "Member"
        };

        private static readonly string[] _userData =
        {
            "Administrator",
            "ChangeMe77!",
            "noreply@company.com"
        };

        public static void Initialize(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            CreateUser(userManager);
            CreateRoles(userManager, roleManager);
        }

        private static void CreateUser(UserManager<IdentityUser> userManager)
        {
            var admin = userManager.Users.FirstOrDefault(u => u.UserName == _userData[0]);
            if (!(admin == null))
                return;

            var user = new IdentityUser()
            {
                UserName = _userData[0],
                Email = _userData[2]
            };

            userManager.CreateAsync(user, _userData[1]).Wait();
        }

        private static void CreateRoles(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            foreach(var r in _roles)
            {
                var role = roleManager.Roles.FirstOrDefault(rn => rn.Name == r);
                if (role == null)
                    roleManager.CreateAsync(new IdentityRole(r)).Wait();
            }

            var admin = userManager.Users.FirstOrDefault(u => u.UserName == _userData[0]);
            if (admin != null)
                userManager.AddToRoleAsync(admin, _roles[0]).Wait();
            else
                throw new Exception("Admin account not found when adding to admin group.");
        }
    }
}

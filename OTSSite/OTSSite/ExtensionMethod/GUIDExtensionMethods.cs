using Microsoft.AspNetCore.Identity;
using OTSSite.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSite.ExtensionMethod
{
    public static class GUIDExtensionMethods
    {
        public static string GetUserName(this Guid guid, UserManager<ApplicationIdentityUser> userManager)
        {
            return userManager.Users.FirstOrDefault(u => u.Id == guid.ToString()).UserName;
        }
    }
}

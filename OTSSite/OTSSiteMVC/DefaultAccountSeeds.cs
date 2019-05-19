using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using OTSSiteMVC.Data;
using OTSSiteMVC.Entities;
using System;
using System.Collections.Generic;
using System.IO;
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

        private static AppIdentityUser SeedAccountData(
            UserManager<AppIdentityUser> _userManager,
            RoleManager<AppIdentityRole> _roleManager)
        {
            //Check and add roles
            foreach(var r in SYSTEM_ROLES)
            {
                if (_roleManager.Roles.Select(role => role.Name).Contains(r))
                    continue;
                _roleManager.CreateAsync(new AppIdentityRole(r)).Wait();           
            }
            //Add email to new user account
            var defaultAccount = new AppIdentityUser(DEFAULT_ACCOUNT_NAME)
            {
                Email = "Administrator@local.host"
            };
            //Create new user
            if(!_userManager.Users.Select(u => u.UserName).Contains(DEFAULT_ACCOUNT_NAME))
                _userManager.CreateAsync(defaultAccount, DEFAULT_ACCOUNT_PASSWORD).Wait();

            _userManager.AddToRolesAsync(defaultAccount, SYSTEM_ROLES).Wait();
            return defaultAccount;
        }
        private static ImageData SeedDefaultImage(ApplicationDbContext dbContext, IHostingEnvironment env)
        {
            var existingImage = dbContext.Images.FirstOrDefault(i => i.FileName == "DefaultProfileImage.png" && i.UserName == "Default");
            if (existingImage != null)
                return existingImage;
            //Set the default iamge path
            var defaultImagePath = Path.Combine(env.WebRootPath, @"Images\default_images\DefaultProfileImage.png");
            //Create default image entry
            var imageData = new ImageData()
            {
                UserId = new Guid(),
                UserName = "Default",
                FileName = "DefaultProfileImage.png",
                ContentType = "image/PNG",
                Image = File.ReadAllBytes(defaultImagePath)
            };
            //Add to dbcontext
            dbContext.Images.Add(imageData);
            //save dbcontext
            dbContext.SaveChanges();
            return dbContext.Images.FirstOrDefault(i => i.FileName == "DefaultProfileImage.png" && i.UserName == "Default");
        }

        public static void Seed(
            UserManager<AppIdentityUser> userManager,
            RoleManager<AppIdentityRole> rolesManager,
            ApplicationDbContext dbContext,
            IHostingEnvironment eng)
        {
            var user = SeedAccountData(userManager, rolesManager); //Seed admin account and roles
            var image = SeedDefaultImage(dbContext, eng);   //Seed default profile image
            user.ImageDataId = image.Id;
            userManager.UpdateAsync(user).Wait();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OTSSite.Entities;
using OTSSite.Models.ViewModels;

namespace OTSSite.Pages.Admin
{
    public class AdministerAccountModel : PageModel
    {
        private readonly UserManager<ApplicationIdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdministerAccountModel(
            UserManager<ApplicationIdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public ModifyAccountViewModel CurrentUserSettings { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId)
        {
            var userEntity = _userManager.Users.FirstOrDefault(u => u.Id == userId);
            var userViewModel = Mapper.Map<ModifyAccountViewModel>(userEntity);
            var userRoles = await _userManager.GetRolesAsync(userEntity);
            var allRoles = _roleManager.Roles.Select(r => r.Name);
            userViewModel.Roles = new Dictionary<string, bool>();
            foreach (var r in allRoles)
            {
                if (userRoles.Contains(r))
                    userViewModel.Roles.Add(r, true);
                else
                    userViewModel.Roles.Add(r, false);
            }
            CurrentUserSettings = userViewModel;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync([FromForm] ModifyAccountViewModel modifyAccountViewModel)
        {
            var user = _userManager.Users
                .FirstOrDefault(u => u.Id == modifyAccountViewModel.UserId);
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach(var r in modifyAccountViewModel.Roles.Keys)
            {
                if (modifyAccountViewModel.Roles[r] == false)
                {
                    if (userRoles.Contains(r))
                        await _userManager.RemoveFromRoleAsync(user, r);
                }
                else
                {
                    if (!userRoles.Contains(r))
                        await _userManager.AddToRoleAsync(user, r);
                }               
            }

            await _userManager.SetLockoutEnabledAsync(user, modifyAccountViewModel.LockoutEnabled);

            return Page();
        }
    }
}
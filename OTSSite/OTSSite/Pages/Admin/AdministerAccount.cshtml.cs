using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OTSSite.Entities;
using OTSSite.Models.ViewModels;

namespace OTSSite.Pages.Admin
{
    [Authorize(Roles = "administrator")]
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
            var userEntity = _userManager.Users.FirstOrDefault(u => u.Id == userId);    //User account object
            var userViewModel = Mapper.Map<ModifyAccountViewModel>(userEntity);         //userViewModel mapped
            var userRoles = await _userManager.GetRolesAsync(userEntity);               //user's assigned Roles
            var allRoles = _roleManager.Roles.Select(r => r.Name);                      //all available roles
            userViewModel.AddRoles(allRoles, userRoles);                                //Adding user Roles to viewmodel.
            CurrentUserSettings = userViewModel;                                        //Applying viewmodel class variable CurrentSettings.
            return Page();
        }
        public async Task<IActionResult> OnPostAsync([FromForm] ModifyAccountViewModel modifyAccountViewModel)
        {
            var user = _userManager.Users
                .FirstOrDefault(u => u.Id == modifyAccountViewModel.UserId);            //The user that is to be modified.
            var userRoles = await _userManager.GetRolesAsync(user) as List<string>;     //All that user's roles
            var availableRoles = _roleManager.Roles;                                    //All available roles
            var modifiedRoles = modifyAccountViewModel.Roles.Keys.ToList();             //modified user roles
            var removeList = new List<string>();                                        //List of roles to remove
            var addList = new List<string>();                                           //List of roles to add

            //Set lockout to whatever is set in the model bind (lazy)
            await _userManager
                .SetLockoutEnabledAsync(user, modifyAccountViewModel.LockoutEnabled);

            //deal with empty roles
            if (modifiedRoles.Count == 0 && userRoles.Count == 0)
            {
                return RedirectToPage(@"AdministerAccounts");
            }

            //deal with 1 set of empty roles
            if (modifiedRoles.Count == 0)
                removeList = userRoles.ToList();
            else if (userRoles.Count == 0)
                addList = modifiedRoles;
            else
            {
                //Add role to removeList by cross referencing the lists
                foreach (var r in userRoles)
                    if (!modifiedRoles.Contains(r))
                        removeList.Add(r);
                //Add roles to the addList by cross referencing hte lists.
                foreach (var r in modifiedRoles)
                    if (!userRoles.Contains(r))
                        addList.Add(r);
            }
            if(removeList.Count == 0 && addList.Count == 0)
                return RedirectToPage(@"AdministerAccounts");
            //Remove roles, throw new exception if failed.
            var remRes = await _userManager.RemoveFromRolesAsync(user, removeList);
            if (!remRes.Succeeded)
                throw new Exception($"Failed to remove roles");
            //Add roles, throw new exception if failed.
            var addRes = await _userManager.AddToRolesAsync(user, addList);
            if (!addRes.Succeeded)
                throw new Exception($"Failed to add roles");
            
            return RedirectToPage(@"AdministerAccounts");
        }
    }
}
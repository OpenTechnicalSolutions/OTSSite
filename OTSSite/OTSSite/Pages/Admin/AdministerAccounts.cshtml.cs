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
    public class AdministerAccountsModel : PageModel
    {
        private readonly UserManager<ApplicationIdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdministerAccountsModel(
            UserManager<ApplicationIdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public List<UserAdminViewModel> Users { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Users = new List<UserAdminViewModel>();                             //Get all users
            var usersFromEntities = _userManager.Users                          //Get users entities
                .ToList();
            //Loop through and add roles to new UserAdminViewModels
            foreach(var u in usersFromEntities)
            {
                var userAdminViewModel = Mapper.Map<UserAdminViewModel>(u);     //Convert ApplicationUserIdentity to AdminUserViewModel
                var roles = await _userManager.GetRolesAsync(u);                //Get User roles
                userAdminViewModel.Roles = roles.ToArray();                     //Add roles to AdminUserViewModel
                Users.Add(userAdminViewModel);                                  //Add AdminUserViewModel to public AdminUserViewModel List Users
            }
            return Page();
        }
    }
}
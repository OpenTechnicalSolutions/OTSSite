using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OTSSite.ViewModel;

namespace OTSSite.Controllers
{
    public class AccountAdministrationController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger _logger;

        public AccountAdministrationController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<AccountAdministrationController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users;
            List<AccountDataViewModel> accountData = new List<AccountDataViewModel>();
            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                var ad = new AccountDataViewModel() { User = u, Roles = roles.ToList() };
                accountData.Add(ad);
            }

            return View(accountData);
        }

        public async Task<IActionResult> Details(string id)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Id == id);
            var roles = await _userManager.GetRolesAsync(user);

            AccountDataViewModel advm = new AccountDataViewModel()
            {
                User = user,
                Roles = roles.ToList()
            };

            return View(advm);
        }

        public async Task<IActionResult> AddRoles(string id)
        {
            //get user
            var user = _userManager.Users.FirstOrDefault(u => u.Id == id);
            //make sure user isn't null
            if (user == null)
                return NotFound("User Not Found!");
            //Existing user roles
            var existingRoles = await _userManager.GetRolesAsync(user);
            //add user Id to AccountRoleViewModel
            var arvm = new AddRolesViewModel()
            {
                UserId = id
            };
            //add existing roles to AccountRoleViewModel
            foreach (var r in existingRoles)
                arvm[r] = true;

            return View(arvm);
        }

        [HttpPost]
        public async Task<IActionResult> AddRoles(AddRolesViewModel arvm)
        {
            //Model state check.
            if (!ModelState.IsValid)
                return View(arvm);
            
            var user = _userManager.Users.FirstOrDefault(u => u.Id == arvm.UserId); //User to add roles to.
            var existingRoles = await _userManager.GetRolesAsync(user);             //Existing roles
            var removeRoles = arvm.RemovedRoles;                                    //Removed roles if enrolled
            var addRoles = arvm.AddedRoles;                                         //Add roles
            var errors = new List<string>();                                        //Errors while adding roles.

            //Remove roles requested
            foreach(var rr in removeRoles)
            {
                if(existingRoles.Contains(rr))
                {
                    var res = await _userManager.RemoveFromRoleAsync(user, rr);
                    if (!res.Succeeded)
                        errors.Add(string.Format("Unable to remove role {0}", rr));
                }
            }
            //Add roles requested
            foreach (var r in addRoles)
            {
                if (!existingRoles.Contains(r))
                {
                    var res = await _userManager.AddToRoleAsync(user, r);
                    if (!res.Succeeded)
                        errors.Add(string.Format("Failed to add role {0}", r));
                }
            }
            //Return if errors.
            if (errors.Count > 0)
            {
                foreach (var e in errors)
                {
                    ModelState.AddModelError("", e);
                }
                return View(arvm);
            }

            return RedirectToAction(nameof(Details), "AccountAdministration", new { Id = arvm.UserId });
        }
    }
}
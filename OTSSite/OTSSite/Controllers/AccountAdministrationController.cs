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

        public IActionResult AddRoles(string id)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Id == id);
            var roles = _roleManager.Roles.Select(rn => rn.Name).ToList();
            if (user == null)
                return NotFound();

            var arvm = new AddRolesViewModel()
            {
                User = user,
                AvailableRoles = roles
            };

            return View(arvm);
        }

        [HttpPost]
        public async Task<IActionResult> AddRoles(AddRolesViewModel arvm)
        {
            if (!ModelState.IsValid)
                return View(arvm);

            var res = new IdentityResult();
            res = await _userManager.AddToRoleAsync(arvm.User, arvm.Role);
            if(!res.Succeeded)
            {
                ModelState.AddModelError("", "Failed to add role due to unknown reason.");
                return View(arvm);
            }

            return RedirectToAction(nameof(Details), arvm.User.Id);
        }
    }
}
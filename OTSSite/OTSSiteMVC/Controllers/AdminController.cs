using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OTSSiteMVC.Entities;
using OTSSiteMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSiteMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly RoleManager<AppIdentityRole> _roleManager;

        public AdminController(
            UserManager<AppIdentityUser> userManager,
            RoleManager<AppIdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "administrator")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userEntities = _userManager.Users;
            var getProfileDtos = new List<GetUserProfileDto>();
            foreach(var u in userEntities)
            {
                var getprofdto = Mapper.Map<GetUserProfileDto>(u);
                getprofdto.Roles = await _userManager.GetRolesAsync(u) as string[];
                getProfileDtos.Add(getprofdto);
            }
            return View(getProfileDtos);
        }
        [Authorize(Roles = "administrator")]
        [HttpGet]
        public async Task<IActionResult> UserConfig(Guid id)
        {
            var userEntity = _userManager.Users.FirstOrDefault(u => u.Id == id);
            if (userEntity == null)
                return NotFound();
            var userConfigDto = Mapper.Map<UserConfigDto>(userEntity);
            userConfigDto.Roles = new Dictionary<string, bool>();
            var assignedRoles = (await _userManager.GetRolesAsync(userEntity)).ToArray();
            var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();
            allRoles.ForEach(r =>
            {
                if (assignedRoles.Contains(r))
                    userConfigDto.Roles.Add(r, true);
                else
                    userConfigDto.Roles.Add(r, false);
            });

            return View(userConfigDto);
        }
        [HttpPost]
        [Authorize(Roles ="administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserConfig([FromForm] UserConfigDto userConfigDto)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Id == userConfigDto.UserId);
            if (user == null)
                return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();
            var addRoles = new List<string>();
            var remRoles = new List<string>();
            allRoles.ForEach(r =>
            {
                if (userConfigDto.Roles[r] && !userRoles.Contains(r))
                    addRoles.Add(r);
                if (!userConfigDto.Roles[r] && userRoles.Contains(r))
                    remRoles.Add(r);
            });
            var res1 = await _userManager.AddToRolesAsync(user, addRoles);
            var res2 = await _userManager.RemoveFromRolesAsync(user, remRoles);
            if (!res1.Succeeded && !res2.Succeeded)
                throw new Exception("Roles failed to be added or removed.");
            user.LockoutEnabled = userConfigDto.Lockout;
            return RedirectToAction("Index");
        }
    }
}

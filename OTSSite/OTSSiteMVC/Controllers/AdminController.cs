using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OTSSiteMVC.Data;
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
        private readonly ApplicationDbContext _dbContext;

        public AdminController(
            UserManager<AppIdentityUser> userManager,
            RoleManager<AppIdentityRole> roleManager,
            ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
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
                getprofdto.ProfileImage = _dbContext.Images.FirstOrDefault(i => i.Id == u.ImageDataId);
                getProfileDtos.Add(getprofdto);
            }
            return View(getProfileDtos);
        }
        [Authorize(Roles = "administrator")]
        [HttpGet]
        public async Task<IActionResult> UserConfig(Guid id)
        {
            //Get user
            var userEntity = _userManager.Users.FirstOrDefault(u => u.Id == id);
            //Confirm user exists
            if (userEntity == null)
                return NotFound();
            //Map user to DTO
            var userConfigDto = Mapper.Map<UserConfigDto>(userEntity);
            //Create new Roles dictionary
            userConfigDto.Roles = new Dictionary<string, bool>();
            //Get Roles user currently has
            var assignedRoles = (await _userManager.GetRolesAsync(userEntity)).ToArray();
            //Get all available roles
            var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();
            //Populate roles dictionary with True or False depending on dictionary.
            allRoles.ForEach(r =>
            {
                if (assignedRoles.Contains(r))
                    userConfigDto.Roles.Add(r, true);
                else
                    userConfigDto.Roles.Add(r, false);
            });

            userConfigDto.ProfileImage = _dbContext.Images.FirstOrDefault(i => i.Id == userEntity.ImageDataId);

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

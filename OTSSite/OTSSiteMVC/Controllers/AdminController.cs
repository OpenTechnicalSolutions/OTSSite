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
            userConfigDto.AssignedRoles = await _userManager.GetRolesAsync(userEntity) as string[];
            if (userConfigDto.AssignedRoles == null)
                userConfigDto.AssignedRoles = new string[0];
            userConfigDto.UnAssignedRoles = _roleManager.Roles
                .Select(r => r.Name)
                .Where(rn => !userConfigDto.AssignedRoles.Contains(rn))
                .ToArray(); 
            return View(userConfigDto);
        }
    }
}

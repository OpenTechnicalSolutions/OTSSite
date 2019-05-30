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
        /// <summary>
        /// Return a view that shows all users
        /// </summary>
        /// <returns>View of all users</returns>
        [Authorize(Roles = "administrator")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userEntities = _userManager.Users;                      //Get all users                                    
            var getProfileDtos = new List<GetUserProfileDto>();         //Create a new list of GetUserProfileDto's
            foreach(var u in userEntities)                              //Loop to add new GetUserProfileDtos to list.
            {
                var getprofdto = Mapper.Map<GetUserProfileDto>(u);      //maps a profile DTO from the entity.
                getprofdto.Roles =                                      //Add current roles to profile dto
                    await _userManager.GetRolesAsync(u) as string[];    
                getprofdto.ProfileImage =                               //Add default profile image to profile dto.
                    _dbContext.Images
                        .FirstOrDefault(i => i.Id == u.ImageDataId);
                getProfileDtos.Add(getprofdto);                         //Add dto to list.
            }
            return View(getProfileDtos);                                //return list
        }
        [Authorize(Roles = "administrator")]
        [HttpGet]
        public async Task<IActionResult> UserConfig(Guid id)
        {
            //Get user
            var userEntity = _userManager.Users
                .FirstOrDefault(u => u.Id == id);
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
            var user = _userManager.Users                               //get user from dto.
                .FirstOrDefault(u => u.Id == userConfigDto.UserId);
            if (user == null)                                           //If null return not found.
                return NotFound();

            var userRoles = await _userManager                          //Get all users roles
                .GetRolesAsync(user);         
            var allRoles = _roleManager.Roles                           //Get all available roles
                .Select(r => r.Name).ToList();
            var addRoles = new List<string>();                          //a list of roles to add
            var remRoles = new List<string>();                          //a list of roles to remove
            allRoles.ForEach(r =>                                       //Loop through all roles and add to add and rem lists.
            {
                if (userConfigDto.Roles[r] && !userRoles.Contains(r))   //if form roles is true and user isn't assigned role 
                    addRoles.Add(r);                                    //add to addRoles list
                if (!userConfigDto.Roles[r] && userRoles.Contains(r))   //if form roles is false and user is assigned role
                    remRoles.Add(r);                                    //add to remRoles list.
            });
            var res1 = await _userManager                               //Add addRoles roles to user and keep result.
                .AddToRolesAsync(user, addRoles);           

            var res2 = await _userManager                               //Rmove remRoles roles from user and keep result.
                .RemoveFromRolesAsync(user, remRoles);

            if (!res1.Succeeded && !res2.Succeeded)                     //If either failed throw exceptions.
                throw new Exception("Roles failed to be added or removed.");
            user.LockoutEnabled = userConfigDto.Lockout;                //Set lockout to what the form says.
            return RedirectToAction("Index");                           //Redirect to Admin dashboard Index.
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OTSSiteMVC.Entities;
using OTSSiteMVC.Models;

namespace OTSSiteMVC.Controllers
{
    [Route("api/Accounts")]
    [ApiController]
    public class AccountApiController : ControllerBase
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly RoleManager<AppIdentityRole> _roleManager;
        private readonly SignInManager<AppIdentityUser> _signInManager;

        public AccountApiController(
            UserManager<AppIdentityUser> userManager,
            RoleManager<AppIdentityRole> roleManager,
            SignInManager<AppIdentityUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        /// <summary>
        /// Register a new account
        /// </summary>
        /// <param name="createUserDto">Dto to create account from</param>
        /// <returns></returns>
        [HttpPost("Register")]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register(CreateUserDto createUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            //Confirm correct content
            //Return Bad request if not.
            if (createUserDto == null)
                return BadRequest();
            if(createUserDto.Password1 != createUserDto.Password2)
                return BadRequest();
            if (createUserDto.Password1 != createUserDto.Password2)
                return BadRequest("The password fields must match.");

            //Map DTO to AppIdentityUser.
            var identityUserEntity = Mapper.Map<AppIdentityUser>(createUserDto);     
            //Create user account.
            var res = await _userManager
                .CreateAsync(identityUserEntity, createUserDto.Password1);
            //Pull user account for redirect.
            var userEntityFromDb = _userManager.Users
                .FirstOrDefault(u => u.UserName == createUserDto.UserName);
            //If create succeeded redirect to User public data.
            if (res.Succeeded)
                return CreatedAtRoute(
                    nameof(GetUserProfile), 
                    new { id = userEntityFromDb.Id }, 
                    userEntityFromDb.Id);
            //Throw new exception if failed.
            throw new Exception($"Failed to create user: {res.Errors}");
        }
        /// <summary>
        /// Gets a user profile
        /// </summary>
        /// <param name="id">user Id</param>
        /// <returns>User profile</returns>
        [HttpGet("Profile/{id}", Name = "GetUserProfile")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserProfile(Guid id)
        {
            //Check if id is null
            if (id == null)
                return BadRequest();
            //Get user.
            var user = _userManager.Users           //GetUser
                .FirstOrDefault(u => u.Id == id);
            //Return NotFound if user doesn't exist.
            if (user == null)
                return NotFound();
            //Convert to userProfileDto
            var userProfileDto = AutoMapper.Mapper.Map<GetUserProfileDto>(user);
            //Add user roles
            var roles = await _userManager
                .GetRolesAsync(user);
            userProfileDto.Roles = roles.ToArray();
            //return user data.
            return Ok(userProfileDto);
        }
        /// <summary>
        /// Login to the system
        /// </summary>
        /// <param name="loginDto">login Dto</param>
        /// <returns></returns>
        [HttpGet("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]LoginDto loginDto)
        {
            //Check model state
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            //Check if loginDto is null
            if (loginDto == null)
                return BadRequest();
            //Get user that matches username.
            var userFromEntity = _userManager.Users
                .FirstOrDefault(u => u.UserName == loginDto.UserName);
            //Attempt the sign in
            var res = await _signInManager
                .PasswordSignInAsync(loginDto.UserName, loginDto.Password, false, false);
            //Return response from sign in.
            if (res.Succeeded)
                return Ok();
            else if (res.IsLockedOut)
                return BadRequest("Account locked.");
            else if (res.IsNotAllowed)
                return BadRequest("Incorrect Username or Password");
            return BadRequest("Sign in failed.");
        }
        /// <summary>
        /// Adds roles to a user account.
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        [HttpPost("AddRoles")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddRoles([FromBody] ModifyUserRolesDto roles)
        {
            ///Get user to add roles to.
            var user = _userManager.Users
                .FirstOrDefault(u => u.Id == roles.UserId);
            //return NotFound if user is null
            if (user == null)
                return NotFound();
            //Get all available roles.
            var availableRoles = _roleManager.Roles
                .Select(r => r.Name)
                .ToList();
            //Get user current Roles.
            var currentRoles = await _userManager.GetRolesAsync(user);
            //Loop through the roles to confirm it's possible to add each role.
            var addRoles = new List<string>();
            roles.Roles
                .ToList()
                .ForEach(r =>
                {
                    if (availableRoles.Contains(r) && !currentRoles.Contains(r))
                        addRoles.Add(r);
                });

            var res = await _userManager.AddToRolesAsync(user, addRoles);
            if(res.Succeeded)
               return CreatedAtRoute(
                    nameof(GetUserProfile),
                    new { id = roles.UserId },
                    roles.UserId);
            throw new Exception("failed to add roles to user.");

        }
    }
}
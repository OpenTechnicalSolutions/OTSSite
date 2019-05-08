using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDto createUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            //Confirm correct content
            //Return Bad request if not.
            if (createUserDto == null)
                return BadRequest();
            //Map DTO to AppIdentityUser.
            var identityUserEntity = Mapper.Map<AppIdentityUser>(createUserDto);     
            //Create user account.
            var res = await _userManager
                .CreateAsync(identityUserEntity, createUserDto.Password);
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
        [HttpGet("Profile/{id}", Name = "GetUserProfile")]
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
        [HttpGet("Login/")]
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
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OTSSiteMVC.Data;
using OTSSiteMVC.Entities;
using OTSSiteMVC.Models;

namespace OTSSiteMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly SignInManager<AppIdentityUser> _signInManager;
        private readonly ApplicationDbContext _dbContext;

        public AccountController(
            UserManager<AppIdentityUser> userManager,
            SignInManager<AppIdentityUser> signInManager,
            ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        /// <summary>
        /// Get a register new View
        /// </summary>
        /// <returns>The view</returns>
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        /// <summary>
        /// Post a register new user form
        /// </summary>
        /// <param name="createUserDto">The form data with the user information</param>
        /// <returns>IActionResult if success or failed</returns>
        [HttpPost]
        public async Task<IActionResult> Register([FromForm] CreateUserDto createUserDto)
        {
            //Confirm if model is valid.
            if (!ModelState.IsValid)
                return View(createUserDto);
            //Confim if passwords match.
            if (createUserDto.Password1 != createUserDto.Password2)
                ModelState.AddModelError("","Password fields must match.");
            //Convert DTO from form data to AppIdentityUser object
            var userEntity = Mapper.Map<AppIdentityUser>(createUserDto);
            //Create user
            var res = await _userManager.CreateAsync(userEntity, createUserDto.Password1);
            //If succeeded redirect to login page.
            if (res.Succeeded)
                return RedirectToAction(nameof(Login));
            //Add errors from result to the ModelState
            var errors = res.Errors.ToList();
            for (int i = 0; i < errors.Count(); i++)
                ModelState.AddModelError(i.ToString(), errors[i].Description);
            //Return DTO with errors.
            return View(createUserDto);
        }
        /// <summary>
        /// Get a Login page View
        /// </summary>
        /// <returns>The view</returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        /// <summary>
        /// Post a login request from a form
        /// </summary>
        /// <param name="loginDto">FormData bound to LoginDto</param>
        /// <returns>A View depending on the result.</returns>
        [HttpPost]
        public async Task <IActionResult> Login([FromForm]LoginDto loginDto)
        {
            //Confirm model state is valid.
            if (!ModelState.IsValid)
                return View(loginDto);
            //Sign in
            var res = await _signInManager.PasswordSignInAsync(loginDto.UserName, loginDto.Password, loginDto.RememberMe, false);
            //If suceeded redirect to home.
            if (res.Succeeded)
                return RedirectToAction("Index", "Home");
            //If locked out return the DTO with that error in the model state.
            else if (res.IsLockedOut)
            {
                ModelState.AddModelError("", "Account is locked out.");
                return View(loginDto);
            }
            //If not allowed return DTO with that error in the Model State
            else if (res.IsNotAllowed)
            {
                ModelState.AddModelError("", "You are not allowed.");
                return View(loginDto);
            }
            //Incorrect username and password. Returned DTO with error added to Model State.
            ModelState.AddModelError("","Username or Password incorrect.");
            return View(loginDto);
        }
        /// <summary>
        /// Logout
        /// </summary>
        /// <returns>Returns home.</returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
           await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// Get a user profile.
        /// </summary>
        /// <param name="username">username of user</param>
        /// <returns>View of profile or NotFound if not found.</returns>
        [HttpGet]
        public async Task<IActionResult> Profile(string username)
        {
            //Get user and return not found if no user.
            var userEntity = _userManager.Users.FirstOrDefault(u => u.UserName == username);
            if (userEntity == null)
                return NotFound();
            //Conver to DTO
            var userProfileDto = Mapper.Map<GetUserProfileDto>(userEntity);
            //Add roles
            userProfileDto.Roles = await _userManager.GetRolesAsync(userEntity) as string[];
            userProfileDto.ProfileImage = _dbContext.Images.FirstOrDefault(i => i.Id == userEntity.ImageDataId);
            return View(userProfileDto);
        }
    }
}
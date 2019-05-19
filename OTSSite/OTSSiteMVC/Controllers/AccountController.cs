using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OTSSiteMVC.Entities;
using OTSSiteMVC.Models;

namespace OTSSiteMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly SignInManager<AppIdentityUser> _signInManager;

        public AccountController(
            UserManager<AppIdentityUser> userManager,
            SignInManager<AppIdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register([FromForm] CreateUserDto createUserDto)
        {
            if (!ModelState.IsValid)
                return View(createUserDto);

            if (createUserDto.Password1 != createUserDto.Password2)
                ModelState.AddModelError("","Password fields must match.");

            var userEntity = Mapper.Map<AppIdentityUser>(createUserDto);
            userEntity.JoinDateTime = DateTime.Now;
            var res = await _userManager.CreateAsync(userEntity, createUserDto.Password1);
            if (res.Succeeded)
                return RedirectToAction(nameof(Login));
            var errors = res.Errors.ToList();
            for (int i = 0; i < errors.Count(); i++)
                ModelState.AddModelError(i.ToString(), errors[i].Description);
                return View(createUserDto);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task <IActionResult> Login([FromForm]LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return View(loginDto);

            var res = await _signInManager.PasswordSignInAsync(loginDto.UserName, loginDto.Password, loginDto.RememberMe, false);
            if (res.Succeeded)
                return RedirectToAction("Index", "Home");
            else if (res.IsLockedOut)
            {
                ModelState.AddModelError("", "Account is locked out.");
                return View(loginDto);
            }
            else if (res.IsNotAllowed)
            {
                ModelState.AddModelError("", "You are not allowed.");
                return View(loginDto);
            }
            ModelState.AddModelError("","Username or Password incorrect.");
            return View(loginDto);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
           await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Profile(string username)
        {
            var userEntity = _userManager.Users.FirstOrDefault(u => u.UserName == username);
            if (userEntity == null)
                return NotFound();
            var userProfileDto = Mapper.Map<GetUserProfileDto>(userEntity);
            userProfileDto.Roles = await _userManager.GetRolesAsync(userEntity) as string[];
            return View(userProfileDto);
        }
    }
}
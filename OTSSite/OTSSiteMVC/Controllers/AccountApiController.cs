using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OTSSiteMVC.Entities;

namespace OTSSiteMVC.Controllers
{
    [Route("api/Accounts")]
    [ApiController]
    public class AccountApiController : ControllerBase
    {
        private readonly UserManager<AppIdentityUser> _userManager;

        public AccountApiController(
            UserManager<AppIdentityUser> userManager)
        {
            _userManager = userManager;
        }
    }
}
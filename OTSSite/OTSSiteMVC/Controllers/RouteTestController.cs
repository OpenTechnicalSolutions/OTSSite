using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OTSSiteMVC.Controllers
{
    [Route("api/RouteTest")]
    [ApiController]
    public class RouteTestController : ControllerBase
    {
        [HttpGet("Test")]
        [AllowAnonymous]
        public IActionResult TestController()
        {
            return Ok(true);
        }
    }
}
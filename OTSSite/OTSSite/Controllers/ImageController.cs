using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OTSSite.Controllers
{
    [Route("api/Images")]
    [ApiController]
    public class ImageController : Controller
    {
        [Route("{userName}/{fileName}")]
        public IActionResult GetImage(string userName, string fileName)
        {
            return Ok();
        }
    }       
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OTSSiteMVC.Data;
using OTSSiteMVC.Entities;
using OTSSiteMVC.Repositories;

namespace OTSSiteMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageApiController : ControllerBase
    {
        private readonly SiteFileRepository _fileRepository;
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public ImageApiController(
            SiteFileRepository fileRepository,
            UserManager<AppIdentityUser> userManager,
            ApplicationDbContext dbContext)
        {
            _fileRepository = fileRepository;
            _userManager = userManager;
            _dbContext = dbContext;
        }
        [HttpPost("Upload")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Author")]
        public async Task<IActionResult> SaveImage([FromBody] IFormFile image)
        {
            if (image == null)
                return BadRequest();

            var userEntity = await _userManager.GetUserAsync(User);
            if (userEntity == null)
                return Unauthorized();

            var imageEntity = new ImageData()
            {
                UserId = userEntity.Id,
                UserName = userEntity.UserName,
                FileName = image.FileName,
                ContentType = image.ContentType
            };

            if (_fileRepository.SaveImage(userEntity.UserName, image))
            {
                _dbContext.Images.Add(imageEntity);
                if (await _dbContext.SaveChangesAsync() < 0)
                    throw new Exception("Failed to save file records");
            }
            else
                throw new Exception("Failed to save file.");
            return CreatedAtRoute("ImageContent", new { userName = userEntity.UserName, fileName = image.FileName }, imageEntity);
        }
        [HttpGet("{userName}/{fileName}")]
        [AllowAnonymous]
        public IActionResult GetImage(string userName, string fileName)
        {
            var image = _dbContext.Images
                .FirstOrDefault(i => i.UserName == userName && i.FileName == fileName);
            if (image == null)
                return NotFound();
            var imageFile = _fileRepository.GetImage(userName, fileName);
            return File(imageFile, image.ContentType);
        }
    }
}
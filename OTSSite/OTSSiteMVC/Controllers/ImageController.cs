using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OTSSiteMVC.Data;
using OTSSiteMVC.Entities;
using OTSSiteMVC.Models;
using static System.Net.Mime.MediaTypeNames;

namespace OTSSiteMVC.Controllers
{
    public class ImageController : Controller
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public ImageController(
            ApplicationDbContext dbContext,
            UserManager<AppIdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }
        [Authorize(Roles = "author, editor, administrator")]
        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }
        [Authorize(Roles = "author, editor, administrator")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Upload([FromForm] UploadImageDto uploadImageDto)
        {
            if (!ModelState.IsValid)
                return View(uploadImageDto);
            var imageDataEntity = Mapper.Map<ImageData>(uploadImageDto);
            _dbContext.Images.Add(imageDataEntity);
            if (_dbContext.SaveChanges() > 0)
                throw new Exception("Failed to save image.");
            return RedirectToAction(nameof(Upload));
        }

        public IActionResult Images(Guid id)
        {
            var imageEntity = _dbContext.Images.FirstOrDefault(i => i.Id == id);
            using (MemoryStream ms = new MemoryStream(imageEntity.Image))
            {
            }
            return View();
        }
    }
}
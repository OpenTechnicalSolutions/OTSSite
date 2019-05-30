using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OTSSiteMVC.Entities;
using TestImageServer.Data;
using TestImageServer.Models;

namespace TestImageServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _dbContext;

        public HomeController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IFormFile file)
        {
            byte[] byteArray;
            using (MemoryStream ms = new MemoryStream())
            {
                file.CopyTo(ms);
                byteArray = ms.ToArray();
            }
            var entity = new ImageData()
            {
                FileName = file.FileName,
                ContentType = file.ContentType,
                Image = byteArray
            };
            _dbContext.Images.Add(entity);
            if (_dbContext.SaveChanges() <= 0)
                throw new Exception("Failed to save image.");
            return RedirectToAction(nameof(Index));
        }
    }
}

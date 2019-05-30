using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestImageServer.Data;
using TestImageServer.Models;

namespace TestImageServer.Controllers
{
    public class ImageController : Controller
    {
        private readonly AppDbContext _dbContext;

        public ImageController(
            AppDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }


        public IActionResult GetImages()
        {
            var imageEntity = _dbContext.Images.ToList();
            var getFileDtos = new List<GetFileDto>();
            imageEntity.ForEach(i =>
            {
                string base64 = Convert.ToBase64String(i.Image);
                string contentString = string.Format("data:{0};base64,{1}", i.ContentType, base64);
                var dto = new GetFileDto()
                {
                    ImageId = i.Id,
                    FileName = i.FileName,
                    ImageBase64 = contentString
                };
                getFileDtos.Add(dto);
            });
            return View(getFileDtos);
        }

        public IActionResult GetImage(Guid id)
        {
            var imageEntity = _dbContext.Images.FirstOrDefault(i => i.Id == id);
            if (imageEntity == null)
                return NotFound();
            return File(imageEntity.Image, imageEntity.ContentType);
        }
    }
}
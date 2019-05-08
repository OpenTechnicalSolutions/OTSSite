using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OTSSite.Entities;
using OTSSite.Models;
using OTSSite.Repositories;

namespace OTSSite.Pages.Authors
{
    [Authorize(Roles = "author")]
    public class ImageHostingModel : PageModel
    {
        private readonly SiteFileRepository _fileRepository;
        private readonly UserManager<ApplicationIdentityUser> _userManager;

        public ImageHostingModel(
            SiteFileRepository fileRepository,
            UserManager<ApplicationIdentityUser> userManager)
        {
            _fileRepository = fileRepository;
            _userManager = userManager;
        }
        
        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost([FromForm] UploadImageDto imageDto)
        {
            if (_userManager.Users.FirstOrDefault(u => u.UserName == imageDto.UserName) == null)
                return Unauthorized();
            foreach(var img in imageDto.ImageFiles)
            if (!_fileRepository.SaveImage(imageDto.UserName, img))
                throw new Exception("Image Upload Failed");
            return RedirectToPage("AuthorDashboard");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OTSSite2.Models;
using OTSSite2.Repositories;

namespace OTSSite.Pages.Comments
{
    public class CommentPageModel : PageModel
    {
        private CommentRepository _commentRepository;
        private UserManager<IdentityUser> _userManager;
        

        public CommentPageModel(CommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public OTSSite2.Models.Comment Comment { get; set; }
        public string Author { get; set; }
        public int Children { get; set; }

        public IActionResult OnGet(OTSSite2.Models.Comment commentObj)
        {
            Comment = commentObj;
            Author = _userManager.Users.FirstOrDefault(u => u.Id == commentObj.AuthorId).UserName;
            Children = _commentRepository.GetChildComments(commentObj.Id).Count();

            return Page();
        }
    }
}
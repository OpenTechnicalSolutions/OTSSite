using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OTSSite.Repositories;

namespace OTSSite.Pages.Article
{
    public class CommentPageModel : PageModel
    {
        private CommentRepository _commentRepository;
        private UserManager<IdentityUser> _userManager;
        

        public CommentPageModel(CommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public string Author { get; set; }
        public string TimeStamp { get; set; }
        public string Comment { get; set; }
        public int Children { get; set; }

        public void OnGet(int articleName)
        {
            
        }
    }
}
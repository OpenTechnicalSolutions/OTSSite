using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OTSSite.Data;
using OTSSite2.Models;
using OTSSite.Repositories;

namespace OTSSite2.Pages.Comments
{
    public class CreateModel : PageModel
    {
        private readonly CommentRepository _commentRepository;
        private UserManager<IdentityUser> _userManager;

        public CreateModel(CommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public IActionResult OnGet(int articleId, int replyId = -1)
        {
            Comment.ArticleId = articleId;
            Comment.AuthorId = _userManager.GetUserId(HttpContext.User);
            Comment.ReplyId = replyId;
            return Page();
        }

        [BindProperty]
        public Comment Comment { get; set; }

        public IActionResult OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Comment.TimeStamp = DateTime.Now;
            _commentRepository.Add(Comment);

            return RedirectToPage("./Index");
        }
    }
}
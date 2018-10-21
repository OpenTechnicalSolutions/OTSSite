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
using OTSSite2.Repositories;

namespace OTSSite2.Pages.Comments
{
    public class CreateModel : PageModel
    {
        private readonly CommentRepository _commentRepository;
        private UserManager<IdentityUser> _userManager;

        public CreateModel(CommentRepository commentRepository, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _commentRepository = commentRepository;
        }
        private int _replyId;
        public async Task<IActionResult> OnGetAsync(int articleId, int replyId = -1)
        {
            Comment.ArticleId = articleId;
            Comment.Author = (await _userManager.GetUserAsync(HttpContext.User)).UserName;
            _replyId = replyId;
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
            Comment.ReplyId = _replyId;
            Comment.TimeStamp = DateTime.Now;
            _commentRepository.Add(Comment);

            return RedirectToPage("./Index");
        }
    }
}
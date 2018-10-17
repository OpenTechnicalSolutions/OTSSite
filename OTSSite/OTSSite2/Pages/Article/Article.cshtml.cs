using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OTSSite.Data;
using OTSSite.Models;
using OTSSite.Repositories;

namespace OTSSite.Pages.Article
{
    public class ArticleModel : PageModel
    {
        private ArticleRepository _articleRepository;
        private UserManager<IdentityUser> _userManager;
        private CommentRepository _commentRepository;

        public ArticleModel(ArticleRepository articleRepository, CommentRepository commentRepository)
        {
            _articleRepository = articleRepository;
            _commentRepository = commentRepository;
        }

        public Models.Article Article { get; set; }
        public IdentityUser Author { get; set; }
        public IEnumerable<Comment> TopLevelComments { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Article = _articleRepository.GetById(id.Value);
            
            if (Article == null)
            {
                return NotFound();
            }

            Author = _userManager.Users.FirstOrDefault(u => u.Id == Article.AuthorId);
            TopLevelComments = _commentRepository.GetTopLevel(Article.Id);

            return Page();
        }
    }
}

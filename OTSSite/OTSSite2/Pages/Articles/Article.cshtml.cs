using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OTSSite.Data;
using OTSSite2.Models;
using OTSSite2.Repositories;
using OTSSite2.VIewModel;

namespace OTSSite.Pages.Articles
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

        public OTSSite2.Models.Article Article { get; set; }
        public IdentityUser Author { get; set; }
        public List<OTSSite2.Models.Comment> TopLevelComments { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //Get Articles
            Article = _articleRepository.GetById(id.Value);
            
            if (Article == null)
            {
                return NotFound();
            }
            //Get Author
            Author = _userManager.Users.FirstOrDefault(u => u.Id == Article.AuthorId);
            //Get top level comments
            TopLevelComments = _commentRepository.GetTopLevel(Article.Id).ToList();        
            return Page();
        }
    }
}

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
using OTSSite2.VIewModel;

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
        public List<CommentViewModel> TopLevelComments { get; set; }

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
            var toplvlcomments = _commentRepository.GetTopLevel(Article.Id);
            //Create list of top level comments
            foreach(var c in toplvlcomments)
            {
                var newcommodel = new CommentViewModel
                {
                    Comment = c,
                    Author = _userManager.Users.FirstOrDefault(com => com.Id == c.AuthorId).UserName,
                    TimeStamp = c.TimeStamp,
                    Children = _commentRepository.GetChildComments(c.ArticleId).Count()
                };
                TopLevelComments.Add(newcommodel);
            }           
            return Page();
        }
    }
}

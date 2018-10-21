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

namespace OTSSite.Pages.Articles
{
    public class ArticleModel : PageModel
    {
        private ArticleRepository _articleRepository;
        private CommentRepository _commentRepository;

        public ArticleModel(ArticleRepository articleRepository, CommentRepository commentRepository)
        {
            _articleRepository = articleRepository;
            _commentRepository = commentRepository;
        }

        public Article Article { get; set; }
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
            //Get top level comments
            TopLevelComments = _commentRepository.GetTopLevel(Article.Id).ToList();        
            return Page();
        }
    }
}

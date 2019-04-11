using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OTSSite.Entities;
using OTSSite.Repositories;

namespace OTSSite.Pages.Publish
{
    public class SubmitArticleModel : PageModel
    {
        private readonly IRepository<Article> _articleRepository;
        private readonly IRepository<Comment> _commentArticle;
        private readonly UserManager<ApplicationIdentityUser> _userManager;

        public SubmitArticleModel(
            IRepository<Article> articleRepository,
            IRepository<Comment> commentRepository,
            UserManager<ApplicationIdentityUser> userManager)
        {
            _articleRepository = articleRepository;
            _commentArticle = commentRepository;
            _userManager = userManager;
        }
        public void OnGet()
        {

        }
    }
}
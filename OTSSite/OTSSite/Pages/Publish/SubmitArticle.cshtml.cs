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
        private readonly ArticleFileRepository _articleFileRepository;

        public SubmitArticleModel(
            IRepository<Article> articleRepository,
            IRepository<Comment> commentRepository,
            UserManager<ApplicationIdentityUser> userManager,
            ArticleFileRepository articleFileRepository)
        {
            _articleRepository = articleRepository;
            _commentArticle = commentRepository;
            _userManager = userManager;
            _articleFileRepository = articleFileRepository;
        }
        public void OnGet()
        {

        }

        public void OnPost()
        {
            var articleFile = Request.Form["Article"];
            var images = Request.Form["Images"];
            var userName = _userManager.GetUserName(User);

            var article = new Article();
            article.AuthorId = _userManager.GetUserId(User);
            article.PublishDate = DateTime.Now;
            article.Title = Request.Form["Title"];
            article.Topic = Request.Form["Topic"];
            article.Path = 
        }
    }
}
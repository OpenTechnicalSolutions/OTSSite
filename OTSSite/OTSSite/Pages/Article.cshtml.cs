using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OTSSite.Entities;
using OTSSite.Repositories;

namespace OTSSite.Pages
{
    public class ArticleModel : PageModel
    {
        private readonly IRepository<Article> _articleRepository;
        private readonly UserManager<ApplicationIdentityUser> _userManager;
        private readonly IRepository<Comment> _commentRepository;

        public ArticleModel(IRepository<Article> articleRepository,
            UserManager<ApplicationIdentityUser> userManager,
            IRepository<Comment> commentRepository)
        {
            _articleRepository = articleRepository;
            _userManager = userManager;
            _commentRepository = commentRepository;
        }

        public class OutModel
        {
            public string Title { get; set; }
            public string AuthorId { get; set; }
            public string AuthorUserName { get; set; }
            public string Topic { get; set; }
            public string PublishDate { get; set; }
            public string ArticleText { get; set; }
        }

        public void OnGet(Guid Id)
        {

        }
    }
}
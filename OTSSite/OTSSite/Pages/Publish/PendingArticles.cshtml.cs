using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OTSSite.Entities;
using OTSSite.Models.ViewModels;
using OTSSite.Repositories;

namespace OTSSite.Pages.Publish
{
    [Authorize(Roles = "editor")]
    public class PendingArticlesModel : PageModel
    {
        private readonly IRepository<Article> _articleRepository;
        private readonly UserManager<ApplicationIdentityUser> _userManager;

        public List<ArticleStatusViewModel> PendingArticleViewModels { get; set; }

        public PendingArticlesModel(
            IRepository<Article> articleRepository, 
            UserManager<ApplicationIdentityUser> userManager)
        {
            _articleRepository = articleRepository;
            _userManager = userManager;
        }
        public IActionResult OnGet()
        {
            var articlesFromEntity = _articleRepository.GetAllPending();
            PendingArticleViewModels = new List<ArticleStatusViewModel>();
            foreach(var a in articlesFromEntity)
            {
                var articlePendingVM = Mapper.Map<ArticleStatusViewModel>(a);
                articlePendingVM.AuthorUserName = _userManager.Users.FirstOrDefault(u => a.AuthorId == u.Id).UserName;
                PendingArticleViewModels.Add(articlePendingVM);
            }
            return Page();
        }
    }
}
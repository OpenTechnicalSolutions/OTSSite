using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OTSSite.Entities;
using OTSSite.Models.ViewModels;
using OTSSite.Repositories;

namespace OTSSite.Pages.Publish
{
    public class DeclinedArticlesModel : PageModel
    {
        private readonly IRepository<Article> _articleRepository;
        private readonly UserManager<ApplicationIdentityUser> _userManager;

        public List<ArticleStatusViewModel> DeclinedArticleViewModels { get; set; }

        public DeclinedArticlesModel(
            IRepository<Article> articleRepository,
            UserManager<ApplicationIdentityUser> userManager)
        {
            _articleRepository = articleRepository;
            _userManager = userManager;
        }
        public IActionResult OnGet()
        {
            var articlesFromEntity = _articleRepository.GetAllDeclined();
            DeclinedArticleViewModels = new List<ArticleStatusViewModel>();
            foreach (var a in articlesFromEntity)
            {
                var articlePendingVM = Mapper.Map<ArticleStatusViewModel>(a);
                articlePendingVM.AuthorUserName = _userManager.Users.FirstOrDefault(u => a.AuthorId == u.Id).UserName;
                DeclinedArticleViewModels.Add(articlePendingVM);
            }
            return Page();
        }
    }
}
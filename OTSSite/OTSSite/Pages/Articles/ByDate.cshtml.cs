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

namespace OTSSite.Pages.Articles
{
    public class ByDateModel : PageModel
    {
        private readonly IRepository<Article> _articleRepository;
        private readonly ArticleFileRepository _articleFileRepository;
        private readonly UserManager<ApplicationIdentityUser> _userManager;

        public ByDateModel(
            IRepository<Article> articleRepository,
            ArticleFileRepository articleFileRepository,
            UserManager<ApplicationIdentityUser> userManager)
        {
            _articleRepository = articleRepository;
            _articleFileRepository = articleFileRepository;
            _userManager = userManager;
        }

        public List<ArticleViewModel> Articles { get; set; }
        public int ArticleCount => Articles.Count();
        public async Task<IActionResult> OnGet(DateTime date, int index = 0)
        {
            var articlesFromEntities = _articleRepository
                .GetByDate(date)
                .OrderByDescending(a => a.PublishDate)
                .ToArray();
            Articles = new List<ArticleViewModel>();
            var max = index + 9 >= articlesFromEntities.Count() ? articlesFromEntities.Count() : index + 9;
            for(var i = index; i < max;i++)
            {
                var articleViewModel = Mapper.Map<ArticleViewModel>(articlesFromEntities[i]);
                articleViewModel.ArticleText = await _articleFileRepository.GetArticle(articlesFromEntities[i].ArticleFile);
                Articles.Add(articleViewModel);
            }
            return Page();
        }
    }
}
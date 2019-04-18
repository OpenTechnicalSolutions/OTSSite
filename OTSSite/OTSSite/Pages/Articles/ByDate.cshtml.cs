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

        IEnumerable<ArticleViewModel> Articles { get; set; }

        public async Task<IActionResult> OnGet(DateTime date)
        {
            var articlesFromEntities = _articleRepository.GetByDate(date);
            Articles = Mapper.Map<IEnumerable<ArticleViewModel>>(articlesFromEntities);
            foreach (var a in Articles)
            {
                a.AuthorUserName = _userManager.Users.FirstOrDefault(u => u.Id == a.AuthorId).UserName;
                var path = articlesFromEntities.FirstOrDefault(art => art.Id == a.ArticleId).ArticleFile;
                a.ArticleText = await _articleFileRepository.GetArticle(path);
            }
            return Page();
        }
    }
}
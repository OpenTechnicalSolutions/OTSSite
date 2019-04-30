using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OTSSite.Entities;
using UsefulExtensionMethods.GeneralExtensions;
using UsefulExtensionMethods.WebExtensions;
using OTSSite.Models.ViewModels;
using OTSSite.Repositories;

namespace OTSSite.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IRepository<Article> _articleRepository;
        private readonly UserManager<ApplicationIdentityUser> _userManager;
        private readonly ArticleFileRepository _articleFileRepository;

        public IndexModel(
            IRepository<Article> articleRepository,
            UserManager<ApplicationIdentityUser> userManager,
            ArticleFileRepository articleFileRepository)
        {
            _articleRepository = articleRepository;
            _userManager = userManager;
            _articleFileRepository = articleFileRepository;
        }

        public List<ArticleViewModel> ArticleViewModels { get; set; }
        public int TotalArticles => ArticleViewModels.Count();

        public async Task<IActionResult> OnGetAsync(int index = 0)
        {
            var articleEntires = _articleRepository
                .GetAll()
                .OrderByDescending(a => a.PublishDate)
                .ToArray();
            ArticleViewModels = new List<ArticleViewModel>();
            var max = index + 9 >= articleEntires.Count() ? articleEntires.Count() : index + 9;
            for(var i = index;i < max;i++)
            {
                var articleViewModel = Mapper.Map<ArticleViewModel>(articleEntires[i]);
                articleViewModel.AuthorUserName = _userManager.Users.FirstOrDefault(u => u.Id == articleEntires[i].AuthorId).UserName;
                articleViewModel.ArticleText = await _articleFileRepository.GetArticle(articleEntires[i].ArticleFile);
                ArticleViewModels.Add(articleViewModel);
            }
            return Page();
        }
    }
}

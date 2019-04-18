using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OTSSite.Entities;
using OTSSite.ExtensionMethods;
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

        public async Task<IActionResult> OnGet()
        {
            var articles = _articleRepository.GetByDate(DateTime.Now).ToList();
            ArticleViewModels = new List<ArticleViewModel>();
            if (articles.Count == 0)
                return Page();

            ArticleViewModels = Mapper.Map<IEnumerable<ArticleViewModel>>(articles).ToList();
            foreach(var a in ArticleViewModels)
            {
                a.AuthorUserName = _userManager.Users.FirstOrDefault(u => u.Id == a.AuthorId).UserName;
                var path = articles.FirstOrDefault(art => art.Id == a.ArticleId).ArticleFile;
                a.ArticleText = await _articleFileRepository.GetArticle(path);
            }
            return Page();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OTSSite.Entities;
using OTSSite.ExtensionMethod;
using OTSSite.Models.ViewModels;
using OTSSite.Repositories;

namespace OTSSite.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IRepository<Article> _articleRepository;
        private readonly UserManager<ApplicationIdentityUser> _userManager;

        public IndexModel(
            IRepository<Article> articleRepository,
            UserManager<ApplicationIdentityUser> userManager)
        {
            _articleRepository = articleRepository;
            _userManager = userManager;
        }

        public List<ArticleViewModel> ArticleViewModels { get; set; }

        public async void OnGet()
        {
            var articles = _articleRepository.GetByDate(DateTime.Now).ToList();
            ArticleViewModels = new List<ArticleViewModel>();
            var articleReader = new ArticleFileReader();
            if (articles.Count == 0)
                return;

            ArticleViewModels = Mapper.Map<IEnumerable<ArticleViewModel>>(articles).ToList();
            foreach(var a in ArticleViewModels)
            {
                var path = articles.FirstOrDefault(art => art.Id == a.ArticleId).ArticleFile;
                a.ArticleText = await articleReader.GetArticle(path);
            }
        }
    }
}

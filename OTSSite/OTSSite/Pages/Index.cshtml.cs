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

        public IndexModel(IRepository<Article> articleRepository,
            UserManager<ApplicationIdentityUser> userManager)
        {
            _articleRepository = articleRepository;
            _userManager = userManager;
        }

        public List<ArticleViewModel> ArticleViewModels { get; set; }

        public void OnGet()
        {
            var articles = _articleRepository.GetByDate(DateTime.Now).ToList();
            var articleViewModels = Mapper.Map<IEnumerable<ArticleViewModel>>(articles);
            var articleFileReader = new ArticleFileReader();

            ArticleViewModels.ForEach(a => {
                a.AuthorUserName = a.AuthorId.GetUserName(_userManager);
                a.ArticleText = articleFileReader.GetArticle(articles.FirstOrDefault(artf => artf.Id == a.ArticleId).ArticleFiles);
            });
        }
    }
}

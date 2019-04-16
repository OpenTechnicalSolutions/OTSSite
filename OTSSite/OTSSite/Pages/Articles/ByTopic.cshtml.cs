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
    public class ByTopicModel : PageModel
    {
        private readonly IRepository<Article> _articleRepository;
        private readonly UserManager<ApplicationIdentityUser> _userManager;
        private readonly ArticleFileRepository _articleFileRepository;

        public ByTopicModel(IRepository<Article> articleRepository,
            UserManager<ApplicationIdentityUser> userManager,
            ArticleFileRepository articleFileRepository)
        {
            _articleRepository = articleRepository;
            _userManager = userManager;
            _articleFileRepository = articleFileRepository;
        }

        public IEnumerable<ArticleViewModel> Articles { get; set; }
        public string Topic { get; set; }

        public async Task<IActionResult> OnGet(string topic)
        {
            Topic = topic;
            var articlesFromEntities = _articleRepository.GetByTopic(topic);
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
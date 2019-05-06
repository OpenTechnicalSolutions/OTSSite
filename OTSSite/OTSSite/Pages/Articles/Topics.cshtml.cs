using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OTSSite.Entities;
using OTSSite.Repositories;

namespace OTSSite.Pages.Articles
{
    public class TopicsModel : PageModel
    {
        private readonly IRepository<Article> _articleRepository; 

        public TopicsModel(IRepository<Article> articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public IEnumerable<String> Topics { get; set; }
        public IActionResult OnGet()
        {
            var topics = _articleRepository
                .GetAll()
                .Where(a => a.Status == Status.Published)
                .Select(a => a.Topic)
                .Distinct();
            Topics = topics;
            return Page();
        }
    }
}
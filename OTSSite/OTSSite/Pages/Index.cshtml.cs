using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OTSSite.Entities;
using OTSSite.Repositories;

namespace OTSSite.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IRepository<Article> _articleRepository;

        public IndexModel(IRepository<Article> articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public List<Article> Articles { get; set; }

        public void OnGet()
        {
            Articles = _articleRepository.GetByDate(DateTime.Now).ToList();
        }
    }
}

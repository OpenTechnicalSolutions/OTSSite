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
    public class ArchiveModel : PageModel
    {
        private readonly IRepository<Article> _articleRepository;

        public ArchiveModel(IRepository<Article> articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public IEnumerable<DateTime> Dates { get; set; }

        public IActionResult OnGet()
        {
            List<DateTime> dates = new List<DateTime>();
            _articleRepository.GetAll()
                .Select(a => a.PublishDate)
                .ToList()
                .ForEach(d => dates[dates.IndexOf(d)] = d.Date);
            Dates = dates.Distinct();

            return Page();
        }
    }
}
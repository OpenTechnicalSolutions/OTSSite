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
            var datesWithoutTime = new List<DateTime>();
            _articleRepository.GetAll()
                .Where(a => a.Status == Status.Published)
                .Select(a => a.PublishDate)
                .OrderByDescending(d => d.Date)
                .ToList()
                .ForEach(d => datesWithoutTime.Add(d.Date));
            Dates = datesWithoutTime.Distinct();

            return Page();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OTSSite.Models;
using OTSSiteRazor.Data;

namespace OTSSiteRazor.Pages.Article
{
    public class IndexModel : PageModel
    {
        private readonly OTSSiteRazor.Data.ApplicationDbContext _context;

        public IndexModel(OTSSiteRazor.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<ArticleModel> ArticleModel { get;set; }

        public async Task OnGetAsync()
        {
            ArticleModel = await _context.ArticleModel.ToListAsync();
        }
    }
}

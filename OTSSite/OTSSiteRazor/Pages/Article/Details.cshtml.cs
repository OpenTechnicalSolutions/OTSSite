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
    public class DetailsModel : PageModel
    {
        private readonly OTSSiteRazor.Data.ApplicationDbContext _context;

        public DetailsModel(OTSSiteRazor.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public ArticleModel ArticleModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ArticleModel = await _context.ArticleModel.FirstOrDefaultAsync(m => m.Id == id);

            if (ArticleModel == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}

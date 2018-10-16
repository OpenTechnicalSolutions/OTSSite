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
    public class DeleteModel : PageModel
    {
        private readonly OTSSiteRazor.Data.ApplicationDbContext _context;

        public DeleteModel(OTSSiteRazor.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ArticleModel = await _context.ArticleModel.FindAsync(id);

            if (ArticleModel != null)
            {
                _context.ArticleModel.Remove(ArticleModel);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

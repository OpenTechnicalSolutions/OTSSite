using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OTSSite.Data;
using OTSSite2.Models;
using OTSSite.Repositories;

namespace OTSSite2.Pages.Articles
{
    public class EditModel : PageModel
    {
        private readonly ArticleRepository _articleRepository;

        public EditModel(ArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        [BindProperty]
        public Article Article { get; set; }

        public IActionResult OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Article = _articleRepository.GetById(id.Value);

            if (Article == null)
            {
                return NotFound();
            }
            return Page();
        }

        public IActionResult OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                _articleRepository.Update(Article);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(Article.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ArticleExists(int id)
        {
            return _articleRepository.Exists(id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OTSSite.Data;
using OTSSite2.Models;
using OTSSite2.Repositories;

namespace OTSSite2.Pages.Articles
{
    public class CreateModel : PageModel
    {
        private readonly ArticleRepository _articleRepository;

        public CreateModel(ArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Article Article { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _articleRepository.Add(Article);

            return RedirectToPage("./Article");
        }
    }
}
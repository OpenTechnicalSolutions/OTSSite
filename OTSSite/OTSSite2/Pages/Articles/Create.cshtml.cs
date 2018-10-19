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
using OTSSite.Repositories;

namespace OTSSite2.Pages.Articles
{
    public class CreateModel : PageModel
    {
        private readonly ArticleRepository _articleRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public CreateModel(ArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public IActionResult OnGet()
        {
            Article.AuthorId = _userManager.GetUserId(HttpContext.User);
            return Page();
        }

        [BindProperty]
        public Article Article { get; set; }

        public IActionResult OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Article.TimeStamp = DateTime.Now;
            _articleRepository.Add(Article);

            return RedirectToPage("./Index");
        }
    }
}
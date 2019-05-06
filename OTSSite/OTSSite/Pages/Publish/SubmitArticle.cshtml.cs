using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OTSSite.Entities;
using OTSSite.Models;
using OTSSite.Repositories;

namespace OTSSite.Pages.Publish
{
    [Authorize(Roles = "author")]
    public class SubmitArticleModel : PageModel
    {
        private readonly IRepository<Article> _articleRepository;
        private readonly IRepository<Comment> _commentArticle;
        private readonly UserManager<ApplicationIdentityUser> _userManager;
        private readonly ArticleFileRepository _articleFileRepository;

        public SubmitArticleModel(
            IRepository<Article> articleRepository,
            IRepository<Comment> commentRepository,
            UserManager<ApplicationIdentityUser> userManager,
            ArticleFileRepository articleFileRepository)
        {
            _articleRepository = articleRepository;
            _commentArticle = commentRepository;
            _userManager = userManager;
            _articleFileRepository = articleFileRepository;
        }
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync([FromForm] CreateArticleDto articleDto)
        {
            if (!ModelState.IsValid)
                return Page();
            var username = _userManager.GetUserName(User);
            var path = await _articleFileRepository.SaveArticle(articleDto.Article, username);
            var articleEntity = Mapper.Map<Article>(articleDto);
            articleEntity.AuthorId = _userManager.GetUserId(User);
            articleEntity.ArticleFile = path;
            _articleRepository.Create(articleEntity);
            if (!_articleRepository.Save())
                throw new Exception("Failed to create article database entry.");
            return RedirectToPage("/Articles/Article", new { id = articleEntity.Id });
        }
    }
}
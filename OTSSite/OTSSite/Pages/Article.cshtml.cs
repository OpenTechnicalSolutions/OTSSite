using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OTSSite.Entities;
using OTSSite.Models.ViewModels;
using OTSSite.Repositories;

namespace OTSSite.Pages
{
    public class ArticleModel : PageModel
    {
        private readonly IRepository<Article> _articleRepository;
        private readonly UserManager<ApplicationIdentityUser> _userManager;
        private readonly IRepository<Comment> _commentRepository;

        public ArticleModel(
            IRepository<Article> articleRepository,
            UserManager<ApplicationIdentityUser> userManager,
            IRepository<Comment> commentRepository)
        {
            _articleRepository = articleRepository;
            _userManager = userManager;
            _commentRepository = commentRepository;
        }

        public ArticleViewModel ArticleViewModel { get; set; }
        public IEnumerable<CommentViewModel> Comments { get; set; }

        public async Task<IActionResult> OnGet(Guid id)
        {
            var articleFromEntity = _articleRepository.Read(id);
            var articleFileReader = new ArticleFileReader();
            if (articleFromEntity == null)
                return NotFound();

            var articleViewModel = Mapper.Map<ArticleViewModel>(articleFromEntity);
            ArticleViewModel.ArticleText = await articleFileReader.GetArticle(articleFromEntity.ArticleFile);

            var commentsFromArticle = _commentRepository.GetByArticle(id).Where(c => c.ParentCommentId == null);
            Comments = Mapper.Map<IEnumerable<CommentViewModel>>(commentsFromArticle);
            return Page();
        }
    }
}
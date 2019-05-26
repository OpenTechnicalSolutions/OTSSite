using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Ganss.XSS;
using Markdig.Parsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OTSSiteMVC.Data;
using OTSSiteMVC.Entities;
using OTSSiteMVC.Models;
using OTSSiteMVC.Repositories;

namespace OTSSiteMVC.Controllers
{
    public class EditorDashboardController : Controller
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly SiteFileRepository _fileRepository;

        public EditorDashboardController(
            UserManager<AppIdentityUser> userManager,
            ApplicationDbContext dbContext,
            SiteFileRepository fileRepository)
        {
            _dbContext = dbContext;
            _fileRepository = fileRepository;
            _userManager = userManager;
        }
        [HttpGet]
        [Authorize(Roles = "editor")]
        public IActionResult Index()
        {
            var pendingArticles = _dbContext.Articles.Where(a => a.Status == Status.Pending);
            var articleInfoDto = Mapper.Map<IEnumerable<ArticleInfoDto>>(pendingArticles);
            return View(articleInfoDto);
        }
        [HttpGet]
        [Authorize(Roles = "editor")]
        public async Task<IActionResult> PreviewArticle(Guid id)
        {
            var htmlSanitizer = new HtmlSanitizer();
            var articleEntity = _dbContext.Articles.FirstOrDefault(a => a.Id == id);
            if (articleEntity == null)
                return NotFound();
            var articleText = await _fileRepository.GetArticle(articleEntity.ArticlePath);
            articleText = htmlSanitizer.SanitizeDocument(MarkdownParser.Parse(articleText).ToString());
            var articleDto = Mapper.Map<GetArticleDto>(articleEntity);
            articleDto.ArticleText = articleText;
            return View(articleDto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "editor")]
        public IActionResult SetStatus(Guid id, int status)
        {
            var articleEntity = _dbContext.Articles.FirstOrDefault(a => a.Id == id);
            if (articleEntity == null)
                return NotFound();
            if (!Enum.IsDefined(typeof(Status), status))
                return BadRequest();
            articleEntity.Status = (Status)status;
            if (_dbContext.SaveChanges() > 0)
                throw new Exception("Unable to save status changes.");
            return View();
        }
    }
}
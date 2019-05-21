using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OTSSiteMVC.Data;
using OTSSiteMVC.Entities;
using OTSSiteMVC.Models;
using OTSSiteMVC.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Markdig.Parsers;
using Ganss.XSS;

namespace OTSSiteMVC.Controllers
{
    public class AuthorDashboardController : Controller
    {
        private readonly SiteFileRepository _fileRepository;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<AppIdentityUser> _userManager;

        public AuthorDashboardController(
            SiteFileRepository fileRepository,
            ApplicationDbContext dbContext,
            UserManager<AppIdentityUser> userManager)
        {
            _fileRepository = fileRepository;
            _dbContext = dbContext;
            _userManager = userManager;
        }
        [Authorize(Roles = "author, editor")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var userArticles = _dbContext.Articles.Where(a => a.AuthorId == user.Id);
            var articleInfoDtos = Mapper.Map<IEnumerable<ArticleInfoDto>>(userArticles);

            return View(articleInfoDtos);
        }
        [HttpGet]
        [Authorize(Roles = "author, editor")]
        public async Task<IActionResult> PreviewArticle(Guid id)
        {
            var htmlSanitizer = new HtmlSanitizer();
            var articleInfo = _dbContext.Articles.FirstOrDefault(a => a.Id == id);
            var articleText = await _fileRepository.GetArticle(articleInfo.ArticlePath);
            articleText = MarkdownParser.Parse(articleText).ToString();
            articleText = htmlSanitizer.SanitizeDocument(articleText);
            return View(articleText);
        }

        [Authorize(Roles = "author, editor")]
        [HttpGet]
        public IActionResult SubmitArticle()
        {
            return View();
        }
        public async Task<IActionResult> SubmitArticle([FromBody] CreateArticleDto createArticleDto)
        {
            var articleEntity = Mapper.Map<Article>(createArticleDto);
            articleEntity.ArticlePath = await _fileRepository.SaveArticle(createArticleDto.ArticleFile, createArticleDto.UserName);
            articleEntity.Status = Status.Pending;
            _dbContext.Articles.Add(articleEntity);
            if (_dbContext.SaveChanges() >= 0)
                return RedirectToAction("Profile", "Account", createArticleDto.UserName);
            throw new Exception("Failed to save article");
        }
    }
}
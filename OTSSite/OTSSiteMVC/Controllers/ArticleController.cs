using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Ganss.XSS;
using Markdig.Parsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OTSSiteMVC.Data;
using OTSSiteMVC.Entities;
using OTSSiteMVC.Models;
using OTSSiteMVC.Repositories;

namespace OTSSiteMVC.Controllers
{
    public class ArticleController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly SiteFileRepository _fileRepository;

        public ArticleController(
            ApplicationDbContext dbContext,
            SiteFileRepository fileRepository)
        {
            _dbContext = dbContext;
            _fileRepository = fileRepository;
        }
        public IActionResult Articles()
        {
            var articleEntities = _dbContext.Articles
                .Where(a => a.Status == Status.Published)
                .OrderBy(a => a.PublishDate);
            var articleInfoDto = Mapper.Map<IEnumerable<ArticleInfoDto>>(articleEntities);
            return View(articleInfoDto);
        }
        public async Task<IActionResult> Article(Guid id)
        {
            var htmlSanitizer = new HtmlSanitizer();
            var articleEntity = _dbContext.Articles
                .FirstOrDefault(a => a.Id == id);
            if (articleEntity == null)
                return NotFound();
            else if (articleEntity.Status != Status.Published)
                return NotFound();
            var articleDto = Mapper.Map<GetArticleDto>(articleEntity);
            var articleText = await _fileRepository.GetArticle(articleEntity.ArticlePath);
            articleText = MarkdownParser.Parse(articleText).ToString();
            articleDto.ArticleText = htmlSanitizer.SanitizeDocument(articleText);
            return View(articleDto);
        }
    }
}
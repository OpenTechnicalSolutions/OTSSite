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
        /// <summary>
        /// Get articles that are published
        /// </summary>
        /// <returns>View with list of articles that are published</returns>
        public IActionResult Articles()
        {
            var articleEntities = _dbContext.Articles                       //Get all articles
                .Where(a => a.Status == Status.Published)                   //that are published
                .OrderBy(a => a.PublishDate);                               //and order by publishdate
            var articleInfoDto =                                            //convert to list of article info dto
                Mapper.Map<IEnumerable<ArticleInfoDto>>(articleEntities);
            return View(articleInfoDto);                                    //return view with list of article info dto
        }
        /// <summary>
        /// Display a published article
        /// </summary>
        /// <param name="id">id of article</param>
        /// <returns>View of article</returns>
        public async Task<IActionResult> Article(Guid id)
        {
            var htmlSanitizer = new HtmlSanitizer();                    //HTML sanitizer
            var articleEntity = _dbContext.Articles                     //Get article
                .FirstOrDefault(a => a.Id == id);                       
            if (articleEntity == null)                                  //if null return NotFound
                return NotFound();
            else if (articleEntity.Status != Status.Published)          //If not published return NotFound
                return NotFound();
            var articleDto =                                            //Map to Dto
                Mapper.Map<GetArticleDto>(articleEntity);
            var articleText = await _fileRepository                     //Load article file
                .GetArticle(articleEntity.ArticlePath);                 
            articleText = MarkdownParser.Parse(articleText).ToString(); //Parse MarkDown
            articleDto.ArticleText = htmlSanitizer                      //Sanatize and add to DTO
                .SanitizeDocument(articleText);
            return View(articleDto);                                    //return view of DTO
        }
    }
}
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
        /// <summary>
        /// Index of EditorDashboard. Lists all articles
        /// </summary>
        /// <returns>View of all articles.</returns>
        [HttpGet]
        [Authorize(Roles = "editor")]
        public IActionResult Index()
        {
            var articles = _dbContext.Articles                      //Get all articles
                .OrderByDescending(a => a.SubmitDate)               //Order descending by submit date
                .ThenByDescending(a => a.Status);                   //Then by descending status.
            var articleInfoDto =                                    //Convert to ArticleInfoDto
                Mapper.Map<IEnumerable<ArticleInfoDto>>(articles);
            return View(articleInfoDto);                            //Return a view of ArticleInfoDto
        }
        /// <summary>
        /// Gets a list of Article Info Dto's by status
        /// </summary>
        /// <param name="status">status to look for</param>
        /// <returns>View of article info list</returns>
        public IActionResult Index(int status)
        {
            if (!Enum.IsDefined(typeof(Status), status))            //If enum is out of index return bad request
                return BadRequest();                                
            var userArticle = _dbContext.Articles                   //Get all articles    
                .Where(a => a.Status == (Status)status)             //that match the status provided
                .OrderByDescending(a => a.SubmitDate);              //Order by submit date descending
            var articleInfoDto =                                    //Map to Article Info Dto list
                Mapper.Map<IEnumerable<ArticleInfoDto>>(userArticle);
            return View(articleInfoDto);                            //return View of article info dtos.
        }
        /// <summary>
        /// Show any article
        /// </summary>
        /// <param name="id">article id</param>
        /// <returns>View of article</returns>
        [HttpGet]
        [Authorize(Roles = "editor")]
        public async Task<IActionResult> PreviewArticle(Guid id)
        {
            var htmlSanitizer = new HtmlSanitizer();                            //HtmlSanitizer       
            var articleEntity = _dbContext.Articles                             //Get articles that matches Id.
                .FirstOrDefault(a => a.Id == id);   
            if (articleEntity == null)                                          //if null return NotFound
                return NotFound();
            var articleText = await _fileRepository                             //Read the article from the file.
                .GetArticle(articleEntity.ArticlePath);
            articleText = htmlSanitizer                                         //Convert article MarkDown and sanitize
                .SanitizeDocument(MarkdownParser.Parse(articleText).ToString());
            var articleDto = Mapper.Map<GetArticleDto>(articleEntity);          //Map article entity to Article Dto
            articleDto.ArticleText = articleText;                               //Add text to article Dto
            return View(articleDto);                                            //Return dto.
        }
        /// <summary>
        /// Update the status of the article
        /// </summary>
        /// <param name="id">article id</param>
        /// <param name="status">new article status</param>
        /// <returns>Redirect to index</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "editor")]
        public IActionResult SetStatus(Guid id, int status)
        {
            var articleEntity = _dbContext.Articles                     //Get article that status needs to be set for
                .FirstOrDefault(a => a.Id == id);
            if (articleEntity == null)                                  //if null return NotFound
                return NotFound();
            if (!Enum.IsDefined(typeof(Status), status))                //If new status is out of range return BadRequest
                return BadRequest();
            articleEntity.Status = (Status)status;                      //Add status to article entity
            articleEntity.PublishDate = DateTime.Now;                   //Set the publish date.
            if (_dbContext.SaveChanges() > 0)                           //save, if failed throw error.
                throw new Exception("Unable to save status changes.");
            return RedirectToAction(nameof(Index));                     //redirect to Editor Dashboard Index.
        }
    }
}
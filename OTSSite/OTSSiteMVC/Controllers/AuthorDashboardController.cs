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
        /// <summary>
        /// Return a list of all articles submitted by user
        /// </summary>
        /// <returns>View of all articles listed by user</returns>
        [Authorize(Roles = "author, editor")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);           //Get current user's database entry
            var userArticles = _dbContext.Articles                      //Get all articles
                .Where(a => a.AuthorId == user.Id)                      //That belong to users
                .OrderBy(a => a.Status)                                 //and order by status
                .ThenByDescending(a => a.SubmitDate);                   //Then order by submit date descending
            var articleInfoDtos =                                       //Convert to article info dto
                Mapper.Map<IEnumerable<ArticleInfoDto>>(userArticles);

            return View(articleInfoDtos);                               //return view of dto
        }
        /// <summary>
        /// Get a list of all articles by this user by status
        /// </summary>
        /// <param name="status">status to search for</param>
        /// <returns>View of status.</returns>
        [HttpGet]
        [Authorize(Roles = "author, editor")]
        public async Task<IActionResult> Index(int status)
        {
            if (!Enum.IsDefined(typeof(Status), status))            //Check to see if enum is in range
                return BadRequest();
            var userEntity = await _userManager                     //Get current user's db entry
                .GetUserAsync(User);
            var userArticle = _dbContext.Articles                   //get all articles
                .Where(a => a.AuthorId == userEntity.Id &&          //written by this user
                            a.Status == (Status)status)             //that match the status provided.
                .OrderByDescending(a => a.SubmitDate);              //and ordered by submit date descending.
            var articleInfoDto =                                    //convert to article Dto
                Mapper.Map<IEnumerable<ArticleInfoDto>>(userArticle);
            return View(articleInfoDto);                            //return View of article info dtos.
        }
        /// <summary>
        /// Preview any article that you have submited
        /// </summary>
        /// <param name="id">article id</param>
        /// <returns>View of article</returns>
        [HttpGet]
        [Authorize(Roles = "author, editor")]
        public async Task<IActionResult> PreviewArticle(Guid id)
        {
            var htmlSanitizer = new HtmlSanitizer();            //HTML Sanitizer
            var userEntity = await _userManager                 //Get logged in user's db entry
                .GetUserAsync(User);
            var articleInfo = _dbContext.Articles               //Get all articles
                .FirstOrDefault(a => a.Id == id &&              //That match the Id in the route
                                a.AuthorId == userEntity.Id);   //and is submited by current user
            if (articleInfo == null)                            //if null return not found
                return NotFound();
            var articleText = await _fileRepository             //Read article file from disk
                .GetArticle(articleInfo.ArticlePath);
            articleText = MarkdownParser                        //parse markdown
                .Parse(articleText)
                .ToString();
            articleText = htmlSanitizer                         //sanitize
                .SanitizeDocument(articleText);
            return View(articleText);                           //return View of Article Dto
        }
        /// <summary>
        /// Return a submit article view
        /// </summary>
        /// <returns>View</returns>
        [Authorize(Roles = "author, editor")]
        [HttpGet]
        public IActionResult SubmitArticle()
        {
            return View();
        }
        /// <summary>
        /// Post a submit article view
        /// </summary>
        /// <param name="createArticleDto">Form data binding to dto</param>
        /// <returns>Redirecto to Author Dashboard index</returns>
        [HttpPost]
        [Authorize(Roles = "author, editor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitArticle([FromBody] CreateArticleDto createArticleDto)
        {
            var articleEntity =                             //Map create article dto to new article entity.
                Mapper.Map<Article>(createArticleDto);
            articleEntity.ArticlePath =                     //save file to disk and add path to entity.
                await _fileRepository
                .SaveArticle(createArticleDto.ArticleFile, createArticleDto.UserName);
            articleEntity.Status = Status.Pending;          //Set status to pending
            _dbContext.Articles.Add(articleEntity);         //Add article to database
            if (_dbContext.SaveChanges() >= 0)              //save and redirect to index if successful
                return RedirectToAction(nameof(Index));
            throw new Exception("Failed to save article");  //throw new exception on failure.
        }
    }
}
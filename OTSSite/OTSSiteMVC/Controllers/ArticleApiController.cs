using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OTSSiteMVC.Data;
using OTSSiteMVC.Entities;
using OTSSiteMVC.Models;
using OTSSiteMVC.Repositories;

namespace OTSSiteMVC.Controllers
{
    [Route("api/ArticleApi")]
    [ApiController]
    public class ArticleApiController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly SiteFileRepository _fileRepository;

        public ArticleApiController(
            ApplicationDbContext dbContext,
            SiteFileRepository siteFileRepository)
        {
            _dbContext = dbContext;
            _fileRepository = siteFileRepository;
        }
        /// <summary>
        /// Gets an article from a Guid for the article Id.
        /// </summary>
        /// <param name="articleId">Article Id Guid</param>
        /// <returns>JSON Object of Article.</returns>
        [HttpGet("{articleId}", Name = "GetArticle")]
        [AllowAnonymous]
        public async Task<IActionResult> GetArticle(Guid articleId)
        {
            //Bad request if no ID specified.
            if (articleId == null)
                return BadRequest();
            //Get article
            var article = _dbContext.Articles
                .FirstOrDefault(a => a.Id == articleId);
            //NotFound if not found.
            if (article == null)
                return NotFound();
            //NotFound if unpublished
            if (article.Status != Status.Published)
                return NotFound();
            //Map Article entity to GetARticleDto
            var getArticleDto = 
                Mapper.Map<GetArticleDto>(article);
            //Read article from disk.
            getArticleDto.ArticleText = 
                await _fileRepository.GetArticle(article.ArticlePath);
            //Return Ok with object.
            return Ok(getArticleDto);
        }
        /// <summary>
        /// Creates an article
        /// </summary>
        /// <param name="createArticleDto">Model bound Article object.</param>
        /// <returns>Success confirmations.</returns>
        [HttpPost()]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "author")]
        public async Task<IActionResult> CreateArticle([FromBody] CreateArticleDto createArticleDto)
        {
            //Check ModelState
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //Check if model bind failed.
            if (createArticleDto == null)
                return BadRequest();
            //Map to entity
            var articleEntity = Mapper.Map<Article>(createArticleDto);
            //Save file and add path to articleEntity
            articleEntity.ArticlePath = 
                await _fileRepository
                .SaveArticle(createArticleDto.ArticleFile, createArticleDto.UserName);
            //Confirm file was saved.
            if (articleEntity.ArticlePath == null)
                return BadRequest();
            //Add articleEntity to database and save database.
            _dbContext.Articles.Add(articleEntity);
            if(!(await _dbContext.SaveChangesAsync() >= 0))
                throw new Exception("Failed to save Article DB Entry.");
            //Convert entity to GetArticleDto
            var articleToReturn = Mapper.Map<GetArticleDto>(articleEntity);
            //Redirect to GetArticle action
            return CreatedAtRoute("GetArticle", new { articleId = articleEntity.Id }, articleToReturn);
        }
    }
}
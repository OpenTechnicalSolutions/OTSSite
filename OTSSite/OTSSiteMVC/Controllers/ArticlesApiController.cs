using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OTSSiteMVC.Data;
using OTSSiteMVC.Entities;
using OTSSiteMVC.Models;
using OTSSiteMVC.Repositories;

namespace OTSSiteMVC.Controllers
{
    [Route("api/Articles")]
    [ApiController]
    public class ArticlesApiController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public ArticlesApiController(
            ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Get a group of articles from a larger set ordered by publish date in descending order.
        /// </summary>
        /// <param name="startIndex">Start index of the set of articles.</param>
        /// <param name="total">How many articles you want.</param>
        /// <returns>A list of GetArticleDtos</returns>
        [HttpGet("{startIndex}/{total}")]
        public IActionResult GetSetOfArticles(int startIndex, int total)
        {
            if (startIndex <= 0 || total <= 0)
                return BadRequest();
            //Get all Article records from DB.
            var allArticleEntities = _dbContext.Articles
                .Where(a => a.Status == Status.Published)
                .OrderByDescending(a => a.PublishDate)
                .ToList();
            //Set the total number of records to return
            //If the total is to big it sets it to the largest possible.
            var count = total;
            if (startIndex + total >= allArticleEntities.Count)
                count = allArticleEntities.Count - startIndex - 1;
            //Adds selected articles to a list.
            var selectedArticles = new List<Article>();
            for (int i = 0; i < count; i++)
                selectedArticles.Add(allArticleEntities[i]);
            //Converts selected article to a GetArticleDto
            //Reads articles from file.
            var selectedArticleDtos = Mapper.Map<IEnumerable<ArticleInfoDto>>(selectedArticles);

            return Ok(selectedArticleDtos);
        }
        /// <summary>
        /// Return articles by author
        /// </summary>
        /// <param name="authorId">authorid</param>
        /// <returns>IEnumerable of articles</returns>
        [HttpGet("Author/{authorId}")]
        public IActionResult ArticlesByAuthor(Guid authorId)
        {
            //Get all articles by author
            var articleEntities = _dbContext.Articles
                .Where(a => a.AuthorId == authorId && a.Status == Status.Published)
                .OrderByDescending(a => a.PublishDate);
            //Convert to ArticleInfoDto
            var articleInfoDtos = Mapper.Map<IEnumerable<ArticleInfoDto>>(articleEntities);
            //Return IEnumerable of ArticleInfoDtos
            return Ok(articleInfoDtos);
        }
        /// <summary>
        /// Return articles by topic
        /// </summary>
        /// <param name="topic">topic</param>
        /// <returns>IEnumerable of Articles</returns>
        [HttpGet("Topic/{topic}")]
        public IActionResult ArticleByTopic(string topic)
        {
            //Get all articles of topic that are published
            var articleEntities = _dbContext.Articles
                .Where(a => a.Topic.Contains(topic) && a.Status == Status.Published)
                .OrderByDescending(a => a.PublishDate);
            //convert to DTO
            var articleInfoDto = Mapper.Map<ArticleInfoDto>(articleEntities);
            //return IEnumerable of DTOs
            return Ok(articleInfoDto);
        }
    }
}
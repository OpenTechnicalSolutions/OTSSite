using Microsoft.EntityFrameworkCore;
using OTSSite.Data;
using OTSSite.Models;
using System.Collections.Generic;
using System.Linq;

namespace OTSSite.Repositories
{
    public class ArticleRepository : IRepository<Article>
    {
        private ApplicationDbContext _dbContext;

        public ArticleRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// Add an article.
        /// </summary>
        /// <param name="item">Article object</param>
        public void Add(Article item)
        {
            _dbContext.Articles.Add(item);
            _dbContext.SaveChanges();
        }
        /// <summary>
        /// Delete an Article
        /// </summary>
        /// <param name="id">Article Id</param>
        public void Delete(int id)
        {
            Delete(GetById(id));
        }
        /// <summary>
        /// Delete an Article
        /// </summary>
        /// <param name="item">Article object</param>
        public void Delete(Article item)
        {
            _dbContext.Articles.Remove(item);
            _dbContext.SaveChanges();
        }
        /// <summary>
        /// Get all Articles
        /// </summary>
        /// <returns>Articles</returns>
        public IEnumerable<Article> GetAll()
        {
            return _dbContext.Articles;
        }
        /// <summary>
        /// Get an Article
        /// </summary>
        /// <param name="id">Article Id</param>
        /// <returns></returns>
        public Article GetById(int id)
        {
            return _dbContext.Articles.FirstOrDefault(a => a.Id == id);
        }
        /// <summary>
        /// Update Article
        /// </summary>
        /// <param name="item">Article object</param>
        public void Update(Article item)
        {
            _dbContext.Attach(item).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
        /// <summary>
        /// Get published Articles
        /// </summary>
        /// <returns>Articles</returns>
        public IEnumerable<Article> GetPublished()
        {
            return GetAll().Where(a => a.Published == true);
        }
        /// <summary>
        /// Get UnPublished
        /// </summary>
        /// <returns>Articles</returns>
        public IEnumerable<Article> GetUnPublished()
        {
            return GetAll().Where(a => a.Published == false);
        }
        /// <summary>
        /// Get published by Author
        /// </summary>
        /// <param name="author">Author username</param>
        /// <returns>Articles</returns>
        public IEnumerable<Article> GetByAuthorPublished(string author)
        {
            return GetPublished().Where(a => a.Author == author);
        }
        /// <summary>
        /// Get unpublished by Author
        /// </summary>
        /// <param name="author">Author username</param>
        /// <returns>Articles</returns>
        public IEnumerable<Article> GetByAuthorUnPublished(string author)
        {
            return GetUnPublished().Where(a => a.Author == author);
        }
        /// <summary>
        /// Check is any article exists
        /// </summary>
        /// <param name="article">Article Id</param>
        /// <returns>bool</returns>
        public bool Exists(int article)
        {
            return _dbContext.Articles.Any(a => a.Id == article);
        }

        public IEnumerable<Article> GetFive(int start = 0)
        {
            var list = new List<Article>();
            var art = _dbContext.Articles.ToList();
            art.OrderByDescending(a => a.TimeStamp);

            if (art.Count <= 5)
                return list;

            for (int i = start; i < start + 5; i++)
                list.Add(art[i]);
            return list;
        }
    }
}

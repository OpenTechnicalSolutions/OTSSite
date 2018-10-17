using OTSSite.Data;
using OTSSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSite.Repositories
{
    public class ArticleRepository : IRepository<Article>
    {
        private ApplicationDbContext _dbContext;

        public ArticleRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Article item)
        {
            _dbContext.Articles.Add(item);
        }

        public void Delete(int id)
        {
            Delete(GetById(id));
        }

        public void Delete(Article item)
        {
            _dbContext.Articles.Remove(item);
        }

        public IEnumerable<Article> GetAll()
        {
            return _dbContext.Articles;
        }

        public Article GetById(int id)
        {
            return _dbContext.Articles.FirstOrDefault(a => a.Id == id);
        }

        public void Update(Article item)
        {
            _dbContext.Articles.Update(item);
        }

        public IEnumerable<Article> GetPublished()
        {
            return GetAll().Where(a => a.Published == true);
        }

        public IEnumerable<Article> GetUnPublished()
        {
            return GetAll().Where(a => a.Published == false);
        }

        public IEnumerable<Article> GetByAuthorPublished(string authorId)
        {
            return GetPublished().Where(a => a.AuthorId == authorId);
        }

        public IEnumerable<Article> GetByAuthorUnPublished(string authorId)
        {
            return GetUnPublished().Where(a => a.AuthorId == authorId);
        }
    }
}

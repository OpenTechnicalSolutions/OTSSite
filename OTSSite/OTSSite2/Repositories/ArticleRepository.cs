using Microsoft.EntityFrameworkCore;
using OTSSite.Data;
using OTSSite2.Models;
using OTSSite2.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSite2.Repositories
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
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            Delete(GetById(id));
        }

        public void Delete(Article item)
        {
            _dbContext.Articles.Remove(item);
            _dbContext.SaveChanges();
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
            _dbContext.Attach(item).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public IEnumerable<Article> GetPublished()
        {
            return GetAll().Where(a => a.Published == true);
        }

        public IEnumerable<Article> GetUnPublished()
        {
            return GetAll().Where(a => a.Published == false);
        }

        public IEnumerable<Article> GetByAuthorPublished(string author)
        {
            return GetPublished().Where(a => a.Author == author);
        }

        public IEnumerable<Article> GetByAuthorUnPublished(string author)
        {
            return GetUnPublished().Where(a => a.Author == author);
        }

        public bool Exists(int article)
        {
            return _dbContext.Articles.Any(a => a.Id == article);
        }
    }
}

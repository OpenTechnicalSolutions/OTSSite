using OTSSite.Data;
using OTSSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSite.Repositories
{
    public class ArticleRepository : IRepository<ArticleModel>
    {
        private ApplicationDbContext _dbContext { get; set; }

        public ArticleRepository(ApplicationDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        public void Add(ArticleModel data)
        {
            _dbContext.Articles.Add(data);
        }

        public void Delete(int id)
        {
            Delete(_dbContext.Articles.FirstOrDefault(a => a.Id == id));
        }

        public void Delete(ArticleModel data)
        {
            _dbContext.Remove(data);
        }

        public IQueryable<ArticleModel> GetAll()
        {
            return _dbContext.Articles;
        }

        public ArticleModel GetById(int id)
        {
            return _dbContext.Articles.FirstOrDefault(a => a.Id == id);
        }

        public void Update(ArticleModel data)
        {
            _dbContext.Articles.Update(data);
        }

        public IQueryable<ArticleModel> GetGroupByTime (DateTime maxDate, int numOfArticles)
        {
            var articles = _dbContext.Articles.Where(a => a.TimeStamp <= maxDate).Take(numOfArticles);
            return articles;
        }
    }
}

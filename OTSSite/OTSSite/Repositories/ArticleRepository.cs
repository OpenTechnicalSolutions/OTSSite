using OTSSite.Data;
using OTSSite.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSite.Repositories
{
    public class ArticleRepository : IRepository<Article>
    {
        private readonly ApplicationDbContext _dbContext;

        public ArticleRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(Article obj)
        {
            _dbContext.Articles.Add(obj as Article);
        }

        public void Delete(Article obj)
        {
            _dbContext.Articles.Remove(obj as Article);
        }

        public Article Read(Guid id)
        {
            dynamic article = _dbContext.Articles.FirstOrDefault(a => a.Id == id);
            return article;
        }

        public void Update(Article obj)
        {
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() >= 0;
        }

        public List<Article> GetByAuthor(Guid authorId)
        {
            return _dbContext.Articles.Where(a => a.Author == authorId).ToList();
        }

        public List<Guid> GetAllGuids()
        {
            return _dbContext.Articles.OrderBy(a => a.PublishDate).Select(a => a.Id).ToList();
        }

        public List<Article> GetByTopic(Guid topicId)
        {
            return _dbContext.Articles.Where(a => a.TopicId == topicId).ToList();
        }

        public List<Article> GetByArticle(Guid articleId)
        {
            throw new NotImplementedException();
        }

        public List<Article> GetByParent(Guid parent)
        {
            throw new NotImplementedException();
        }

        public List<Article> GetByDate(DateTime date)
        {
            var dt = date.Date;
            return _dbContext.Articles.Where(a => a.PublishDate >= dt && a.PublishDate < dt.AddDays(1)).ToList();
        }
    }
}

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
            var article = _dbContext.Articles
                .FirstOrDefault(a => a.Id == id && a.Status == Status.Published);
            return article;
        }

        public void Update(Article obj)
        {
            System.Diagnostics.Debug.WriteLine("The database has been udpated.");
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() >= 0;
        }

        public IEnumerable<Article> GetByAuthor(string authorId)
        {
            return _dbContext.Articles
                .Where(a => a.AuthorId == authorId && a.Status == Status.Published);
        }

        public IEnumerable<Guid> GetAllGuids()
        {
            return _dbContext.Articles
                .Where(a => a.Status == Status.Published)
                .OrderByDescending(a => a.PublishDate)
                .Select(a => a.Id);
        }

        public IEnumerable<Article> GetByTopic(string topic)
        {
            return _dbContext.Articles
                .Where(a => a.Topic == topic);
        }

        public IEnumerable<Article> GetByDate(DateTime date)
        {
            var dt = date.Date;
            return _dbContext.Articles
                .Where(a => a.PublishDate >= dt && a.PublishDate < dt.AddDays(1) && a.Status == Status.Published);
        }

        public IEnumerable<Article> GetAll()
        {
            return _dbContext.Articles.ToList();
        }

        public IEnumerable<Article> GetByArticle(Guid articleId)
        {
            throw new NotImplementedException("GetByArticle may only be used by a CommentRepository.");
        }

        public IEnumerable<Article> GetByParent(Guid parent)
        {
            throw new NotImplementedException("GetByParent  may only be used by a CommentRepository.");
        }

        public IEnumerable<Article> GetAllPending()
        {
            return _dbContext.Articles
                .Where(a => a.Status == Status.Pending);           
        }

        public Article GetAny(Guid id)
        {
            return _dbContext.Articles.FirstOrDefault(a => a.Id == id);
        }

        public IEnumerable<Article> GetAllDeclined()
        {
            return _dbContext.Articles
                .Where(a => a.Status == Status.Declined);
        }
    }
}

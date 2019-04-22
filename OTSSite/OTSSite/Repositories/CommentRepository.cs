using OTSSite.Data;
using OTSSite.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSite.Repositories
{
    public class CommentRepository : IRepository<Comment>
    {
        private readonly ApplicationDbContext _dbContext;

        public CommentRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(Comment obj)
        {
            _dbContext.Comments.Add(obj as Comment);
        }

        public void Delete(Comment obj)
        {
            _dbContext.Comments.Remove(obj as Comment);
        }

        public IEnumerable<Guid> GetAllGuids()
        {
            return _dbContext.Comments.OrderBy(c => c.PublishDate).Select(c => c.Id);
        }

        public IEnumerable<Comment> GetByArticle(Guid articleId)
        {
            return _dbContext.Comments.Where(c => c.Id == articleId).OrderBy(c => c.PublishDate);
        }

        public IEnumerable<Comment> GetByParent(Guid parent)
        {
            return _dbContext.Comments.Where(c => c.ParentCommentId == parent);
        }

        public Comment Read(Guid id)
        {
            dynamic comment = _dbContext.Comments.Where(c => c.Id == id);
            return comment;
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() >= 0;
        }

        public void Update(Comment obj)
        {
            System.Diagnostics.Debug.WriteLine("The database has been udpated.");
        }

        public IEnumerable<Comment> GetByAuthor(string authorId)
        {
            throw new NotImplementedException("GetByAuthor may only be used by an ArticleRepository.");
        }

        public IEnumerable<Comment> GetByDate(DateTime date)
        {
            throw new NotImplementedException("GetByDate may only be used by an ArticleRepository.");
        }

        public IEnumerable<Comment> GetByTopic(string topic)
        {
            throw new NotImplementedException("GetByTopic may only be used by an ArticleRepository.");
        }

        public IEnumerable<Comment> GetAll()
        {
            throw new NotImplementedException("GetAll may only be used by an ArticleRepository.");
        }

        public IEnumerable<Comment> GetAllPending()
        {
            throw new NotImplementedException();
        }

        public Comment GetAny(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Comment> GetAllDeclined()
        {
            throw new NotImplementedException();
        }
    }
}

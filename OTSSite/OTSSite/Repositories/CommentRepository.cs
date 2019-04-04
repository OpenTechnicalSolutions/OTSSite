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

        public List<Guid> GetAllGuids()
        {
            return _dbContext.Comments.OrderBy(c => c.PublishDate).Select(c => c.Id).ToList();
        }

        public List<Comment> GetByArticle(Guid articleId)
        {
            return _dbContext.Comments.Where(c => c.Id == articleId).OrderBy(c => c.PublishDate).ToList();
        }

        public List<Comment> GetByParent(Guid parent)
        {
            return _dbContext.Comments.Where(c => c.ParentCommentId == parent).ToList();
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

        public List<Comment> GetByAuthor(Guid authorId)
        {
            throw new NotImplementedException();
        }

        public List<Comment> GetByDate(DateTime date)
        {
            throw new NotImplementedException();
        }

        public List<Comment> GetByTopic(string topic)
        {
            throw new NotImplementedException();
        }
    }
}

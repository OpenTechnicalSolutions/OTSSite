using OTSSite.Data;
using OTSSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSite.Repositories
{
    public class CommentRepository : IRepository<Comment>
    {
        public ApplicationDbContext _dbContext;

        public CommentRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Comment item)
        {
            _dbContext.Comments.Add(item);
        }

        public void Delete(int id)
        {
            Delete(_dbContext.Comments.FirstOrDefault(c => c.Id == id));
        }

        public void Delete(Comment item)
        {
            _dbContext.Comments.Remove(item);
        }

        public IEnumerable<Comment> GetAll()
        {
            return _dbContext.Comments;
        }

        public Comment GetById(int id)
        {
            return GetAll().FirstOrDefault(c => c.Id == id);
        }

        public void Update(Comment item)
        {
            _dbContext.Comments.Update(item);
        }

        public IEnumerable<Comment> GetByArticleId(int articleId)
        {
            return GetAll().Where(c => c.ArticleId == articleId);
        }

        public IEnumerable<Comment> GetTopLevel(int articleId)
        {
            return GetByArticleId(articleId).Where(c => c.ReplyId == 0);
        }

        public IEnumerable<Comment> GetChildComments(int replyId)
        {
            return GetAll().Where(c => c.ReplyId == replyId);
        }
    }
}

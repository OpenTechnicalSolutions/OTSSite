using OTSSite.Data;
using OTSSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSite.Repositories
{
    public class CommentRepository : IRepository<CommentModel>
    {
        private ApplicationDbContext _dbContext;

        public CommentRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(CommentModel data)
        {
            _dbContext.Comments.Add(data);
        }

        public void Delete(int id)
        {
            Delete(_dbContext.Comments.FirstOrDefault(c => c.Id == id));
        }

        public void Delete(CommentModel data)
        {
            Delete(data);
        }

        public IQueryable<CommentModel> GetAll()
        {
            return (_dbContext.Comments);
        }

        public CommentModel GetById(int id)
        {
            return (_dbContext.Comments.FirstOrDefault(c => c.Id == id));
        }

        public void Update(CommentModel data)
        {
            _dbContext.Comments.Update(data);
        }

        public IQueryable<CommentModel> GetArticleComments(int articleId)
        {
            return (_dbContext.Comments.Where(c => c.Id == articleId));
        }

        public IQueryable<CommentModel> GetTopLevelComments(int articleId)
        {
            return (GetArticleComments(articleId).Where(c => c.ReplyCommentId == -1));
        }

        public IQueryable<CommentModel> GetReplyComments(int replyCommentId)
        {
            return (_dbContext.Comments.Where(c => c.ReplyCommentId == replyCommentId));
        }
    }
}

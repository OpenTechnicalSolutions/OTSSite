using OTSSite.Data;
using OTSSite.Models;
using System.Collections.Generic;
using System.Linq;

namespace OTSSite.Repositories
{
    public class CommentRepository : IRepository<Comment>
    {
        public ApplicationDbContext _dbContext;

        public CommentRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// Add a comment
        /// </summary>
        /// <param name="item">Comment object</param>
        public void Add(Comment item)
        {
            _dbContext.Comments.Add(item);
            _dbContext.SaveChanges();
        }
        /// <summary>
        /// Delete comment
        /// </summary>
        /// <param name="id">Comment Id</param>
        public void Delete(int id)
        {
            Delete(_dbContext.Comments.FirstOrDefault(c => c.Id == id));

        }/// <summary>
        /// Delete comment
        /// </summary>
        /// <param name="item">Comment object</param>
        public void Delete(Comment item)
        {
            _dbContext.Comments.Remove(item);
            _dbContext.SaveChanges();
        }
        /// <summary>
        /// Get all comments
        /// </summary>
        /// <returns>Comments</returns>
        public IEnumerable<Comment> GetAll()
        {
            return _dbContext.Comments;
        }
        /// <summary>
        /// Get Comment by Id
        /// </summary>
        /// <param name="id"Comment Id></param>
        /// <returns>A Comment</returns>
        public Comment GetById(int id)
        {
            return GetAll().FirstOrDefault(c => c.Id == id);
        }
        /// <summary>
        /// Update an existing Comment
        /// </summary>
        /// <param name="item">Comment object</param>
        public void Update(Comment item)
        {
            _dbContext.Comments.Update(item);
            _dbContext.SaveChanges();
        }
        /// <summary>
        /// Get Article Replies
        /// </summary>
        /// <param name="articleId">Article Id</param>
        /// <returns>Comments</returns>
        public IEnumerable<Comment> GetByArticleId(int articleId)
        {
            return GetAll().Where(c => c.ArticleId == articleId);
        }
        /// <summary>
        /// Get top level Replies
        /// </summary>
        /// <param name="articleId"Article Id></param>
        /// <returns>Comments</returns>
        public IEnumerable<Comment> GetTopLevel(int articleId)
        {
            return GetByArticleId(articleId).Where(c => c.ReplyId == 0);
        }
        /// <summary>
        /// Get Comment replies
        /// </summary>
        /// <param name="replyId">Comment Id</param>
        /// <returns>Comments</returns>
        public IEnumerable<Comment> GetChildComments(int replyId)
        {
            return GetAll().Where(c => c.ReplyId == replyId);
        }
    }
}

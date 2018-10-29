using OTSSite.Data;
using OTSSite.Models;
using System.Linq;

namespace OTSSite.Repositories
{
    public class TopicRepository
    {
        public ApplicationDbContext _applicationDbContext;

        public TopicRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        /// <summary>
        /// Get Article Topics
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns>Topics</returns>
        public string[] GetTopics (int articleId)
        {
            return _applicationDbContext.Topics.Where(t => t.ArticleId == articleId).Select(t => t.Name).ToArray();
        }
        /// <summary>
        /// Get Article Ids in Topic
        /// </summary>
        /// <param name="topic">Topic</param>
        /// <returns>Article Ids</returns>
        public int[] GetArticleIds(string topic)
        {
            return _applicationDbContext.Topics.Where(t => t.Name == topic).Select(t => t.ArticleId).ToArray();
        }
        /// <summary>
        /// Assign a topic to an Article
        /// </summary>
        /// <param name="topic">Topic</param>
        /// <param name="articleId">Article Id</param>
        public void AssignTopic(string topic, int articleId)
        {
            _applicationDbContext.Topics.Add(new Topic() { Name = topic, ArticleId = articleId });
        }
    }
}

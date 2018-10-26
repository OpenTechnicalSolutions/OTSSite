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

        public string[] GetTopics (int articleId)
        {
            return _applicationDbContext.Topics.Where(t => t.ArticleId == articleId).Select(t => t.Name).ToArray();
        }

        public int[] GetArticleIds(string topic)
        {
            return _applicationDbContext.Topics.Where(t => t.Name == topic).Select(t => t.ArticleId).ToArray();
        }

        public void AddTopic(string topic, int articleId)
        {
            _applicationDbContext.Topics.Add(new Topic() { Name = topic, ArticleId = articleId });
        }
    }
}

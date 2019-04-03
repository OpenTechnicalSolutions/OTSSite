using OTSSite.Data;
using OTSSite.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSite.Repositories
{
    public class TopicRepository : IRepository<Topic>
    {
        private readonly ApplicationDbContext _dbContext;

        public TopicRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(Topic obj)
        {
            _dbContext.Topics.Add(obj);
        }

        public void Delete(Topic obj)
        {
            _dbContext.Topics.Remove(obj);
        }

        public List<Guid> GetAllGuids()
        {
            return _dbContext.Topics.Select(t => t.Id).ToList();
        }

        public Topic Read(Guid id)
        {
            return _dbContext.Topics.FirstOrDefault(t => t.Id == id);
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() >= 0;
        }

        public void Update(Topic obj)
        {
            System.Diagnostics.Debug.WriteLine("The database has been udpated.");
        }

        public List<Topic> GetByArticle(Guid articleId)
        {
            throw new NotImplementedException();
        }

        public List<Topic> GetByAuthor(Guid authorId)
        {
            throw new NotImplementedException();
        }

        public List<Topic> GetByDate(DateTime date)
        {
            throw new NotImplementedException();
        }

        public List<Topic> GetByParent(Guid parent)
        {
            throw new NotImplementedException();
        }

        public List<Topic> GetByTopic(Guid topicId)
        {
            throw new NotImplementedException();
        }
    }
}

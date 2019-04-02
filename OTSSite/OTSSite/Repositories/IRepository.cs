using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSite.Repositories
{
    public interface IRepository<T> where T : class
    {
        void Create(T obj);
        T Read(Guid id);
        void Update(T obj);
        void Delete(T obj);
        bool Save();
        List<T> GetByAuthor(Guid authorId);
        List<Guid> GetAllGuids();
        List<T> GetByParent(Guid parent);
        List<T> GetByArticle(Guid articleId);
        List<T> GetByTopic(Guid topicId);
        List<T> GetByDate(DateTime date);
    }
}

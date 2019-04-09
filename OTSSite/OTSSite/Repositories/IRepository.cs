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
        List<T> GetByAuthor(string authorId);
        List<Guid> GetAllGuids();
        List<T> GetByParent(Guid parent);
        List<T> GetByArticle(Guid articleId);
        List<T> GetByTopic(string topic);
        List<T> GetByDate(DateTime date);
    }
}

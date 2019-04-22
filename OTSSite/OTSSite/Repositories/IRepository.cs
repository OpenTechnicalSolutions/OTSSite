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
        IEnumerable<T> GetByAuthor(string authorId);
        IEnumerable<Guid> GetAllGuids();
        IEnumerable<T> GetByParent(Guid parent);
        IEnumerable<T> GetByArticle(Guid articleId);
        IEnumerable<T> GetByTopic(string topic);
        IEnumerable<T> GetByDate(DateTime date);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAllPending();
        T GetAny(Guid id);
        IEnumerable<T> GetAllDeclined();
    }
}

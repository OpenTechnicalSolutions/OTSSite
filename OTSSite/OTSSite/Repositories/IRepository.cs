using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSite.Repositories
{
    public interface IRepository<T> where T : class
    {
        void Add(T data);
        void Update(T data);
        IQueryable<T> GetAll();
        T GetById(int id);
        void Delete(int id);
        void Delete(T data);
    }
}

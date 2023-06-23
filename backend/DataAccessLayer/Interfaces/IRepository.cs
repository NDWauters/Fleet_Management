using System.Collections.Generic;

namespace DataAccessLayer.Interfaces
{
    public interface IRepository<T> where T : class 
    {
        List<T> GetAll();
        T Get(int id);
        int Add(T entity);
        int Update(T entity);
        void Delete(int id);
    }
}

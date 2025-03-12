using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookDiary.DataAccess.Repository
{
    public interface IRepository <T> where T : class
    {
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(int id);
        Task<T> Get(int id);
        IQueryable<T> GetAll();
        Task<T> GetById(int id);
        Task< List<T>> Find(Expression<Func<T, bool>> filter);
        Task<T> Get(Expression<Func<T, bool>> filter);
    }
}

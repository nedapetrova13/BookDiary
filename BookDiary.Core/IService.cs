using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookDiary.Core
{
    public interface IService<T> where T : class
    {
        void Add(Task entity);
        void Update(T entity);
        void Delete(int id);
        List<T> GetAll();
        T GetById(int id);
        List<T> Find(Expression<Func<T, bool>> filter);
    }
    
        
    
}

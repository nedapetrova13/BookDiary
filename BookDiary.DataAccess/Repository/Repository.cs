using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BookDiary.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext context)
        {
            this._context = context;
            this.dbSet = _context.Set<T>();

        }
        public void Add(T entity)
        {
            this.dbSet.Add(entity);
            this._context.SaveChanges();
        }

        public void Delete(int id)
        {
            if (id == null)
            {
                throw new ArgumentException("Id is null!");
            }
            T entitiy= dbSet.Find(id);
            this.dbSet.Remove(entitiy);
            this._context.SaveChanges();
        }

        public List<T> Find(Expression<Func<T, bool>> filter)
        {
            return dbSet.Where(filter).ToList();
        }

        public T Get(int id)
        {
            if (id == null)
            {
                throw new ArgumentException("id is null");
            }
            T entity = dbSet.Find(id);
            return entity;
        }

        public List<T> GetAll()
        {
            return dbSet.ToList();
        }

        public void Update(T entity)
        {
            dbSet.Update(entity);
        }
    }
}

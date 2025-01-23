using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BookDiary.Core.IServices;
using BookDiary.DataAccess.Repository;
using BookDiary.Models;

namespace BookDiary.Core.Services
{
    public class NewsService : INewsService
    {
        private readonly IRepository<News> _repo;
        public NewsService(IRepository<News> repo)
        {
            this._repo = repo;
        }

        public void Add(Task entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<News> Find(Expression<Func<News, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public List<News> GetAll()
        {
            throw new NotImplementedException();
        }

        public News GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(News entity)
        {
            throw new NotImplementedException();
        }
    }
}

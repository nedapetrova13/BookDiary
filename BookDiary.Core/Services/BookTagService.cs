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
    public class BookTagService : IBookTagService
    {
        private readonly IRepository<BookTag> _repo;
        public BookTagService(IRepository<BookTag> repo)
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

        public List<BookTag> Find(Expression<Func<BookTag, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public List<BookTag> GetAll()
        {
            throw new NotImplementedException();
        }

        public BookTag GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(BookTag entity)
        {
            throw new NotImplementedException();
        }
    }
}

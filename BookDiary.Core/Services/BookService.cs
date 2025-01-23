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
    public class BookService : IBookService
    {
        private readonly IRepository<Book> _repo;
        public BookService(IRepository<Book> repo)
        {
            this._repo = repo;
        }

        public Task Add(Task entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Book>> Find(Expression<Func<Book, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Task<List<Book>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Book> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(Book entity)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
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

        public async Task Add(Book entity)
        {
            await _repo.Add(entity);
        }

        public async Task Delete(int id)
        {
            await _repo.Delete(id);
        }

        public async Task<List<Book>> Find(Expression<Func<Book, bool>> filter)
        {
            return await _repo.Find(filter);
        }

        public async Task<List<Book>> GetAll()
        {
           return await _repo.GetAll();
        }

        public Task<Book> GetById(int id)
        {
            return _repo.GetById(id);
        }

        public async Task Update(Book entity)
        {
           await _repo.Update(entity);
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await _repo.GetAll();
        }
    }
}

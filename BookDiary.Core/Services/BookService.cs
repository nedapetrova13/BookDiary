using System;
using System.Collections.Generic;
using System.Drawing.Text;
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
        private readonly IRepository<BookTag> _btrepo;
        private readonly IRepository<Tag> _tagrepo;

        public BookService(IRepository<Book> repo, IRepository<BookTag> btrepo, IRepository<Tag> tagrepo)
        {
            this._repo = repo ?? throw new ArgumentNullException(nameof(repo));
            this._btrepo = btrepo ?? throw new ArgumentNullException(nameof(btrepo));
            this._tagrepo = tagrepo ?? throw new ArgumentNullException(nameof(tagrepo));
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

      
        public Task<Book> GetById(int id)
        {
            return _repo.GetById(id);
        }

        public async Task Update(Book entity)
        {
           await _repo.Update(entity);
        }

        
        public Task AddTagToBook(string bookname, string tagname)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Book> GetAll()
        {
            return  _repo.GetAll();
        }

        public Task<Book> Get(Expression<Func<Book, bool>> filter)
        {
            throw new NotImplementedException();
        }

       
    }
}

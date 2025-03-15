using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
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

        public async Task Add(BookTag entity)
        {
            await _repo.Add(entity);
        }

        public async Task Delete(int id)
        {
            await _repo.Delete(id);
        }

        public async Task DeleteBookTag(int bookId, int tagId)
        {
            await _repo.DeleteMapping<BookTag>(bt => bt.BookId == bookId && bt.TagId == tagId);
        }

        public async Task<List<BookTag>> Find(Expression<Func<BookTag, bool>> filter)
        {
            return await _repo.Find(filter);
        }

        public Task<BookTag> Get(Expression<Func<BookTag, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public IQueryable<BookTag> GetAll()
        {
            return  _repo.GetAll();
        }

        public async Task<BookTag> GetById(int id)
        {
            return await _repo.GetById(id);
        }

        public async Task Update(BookTag entity)
        {
            await _repo.Update(entity);
        }

       
    }
}

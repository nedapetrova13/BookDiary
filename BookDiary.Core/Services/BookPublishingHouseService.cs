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
    public class BookPublishingHouseService : IBookPublishingHouseService
    {
        private readonly IRepository<BookPublishingHouse> _repo;
        public BookPublishingHouseService(IRepository<BookPublishingHouse> repo)
        {
            this._repo = repo;
        }

        public async Task Add(BookPublishingHouse entity)
        {
            await _repo.Add(entity);
        }

        public async Task Delete(int id)
        {
            await _repo.Delete(id);
        }

        public async Task<List<BookPublishingHouse>> Find(Expression<Func<BookPublishingHouse, bool>> filter)
        {
            return await _repo.Find(filter);
        }

        public Task<BookPublishingHouse> Get(Expression<Func<BookPublishingHouse, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public IQueryable<BookPublishingHouse> GetAll()
        {
            return  _repo.GetAll();
        }

        public async Task<BookPublishingHouse> GetById(int id)
        {
            return await _repo.GetById(id);
        }

        public async Task Update(BookPublishingHouse entity)
        {
            await _repo.Update(entity); 
        }

       
    }
}

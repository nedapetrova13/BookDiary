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
    public class ShelfService : IShelfService
    {
        private readonly IRepository<Shelf> _repo;
        public ShelfService(IRepository<Shelf> repo)
        {
            this._repo = repo;
        }

        public async Task Add(Shelf entity)
        {
            await _repo.Add(entity);
        }

        public async Task Delete(int id)
        {
            await _repo.Delete(id);
        }

        public async Task<List<Shelf>> Find(Expression<Func<Shelf, bool>> filter)
        {
            return await _repo.Find(filter);
        }

        public async Task<Shelf> Get(Expression<Func<Shelf, bool>> filter)
        {
            return await _repo.Get(filter);
        }

        public IQueryable<Shelf> GetAll()
        {
            return  _repo.GetAll();
        }

        public async Task<Shelf> GetById(int id)
        {
            return await _repo.GetById(id);
        }

        public async Task Update(Shelf entity)
        {
            await _repo.Update(entity);
        }

        
    }
}

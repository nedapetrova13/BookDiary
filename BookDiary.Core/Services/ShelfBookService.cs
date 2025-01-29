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
    public class ShelfBookService : IShelfBookService
    {
        private readonly IRepository<ShelfBook> _repo;
        public ShelfBookService(IRepository<ShelfBook> repo)
        {
            this._repo = repo;
        }

        public async Task Add(ShelfBook entity)
        {
            await _repo.Add(entity);
        }

        public async Task Delete(int id)
        {
            await _repo.Delete(id);
        }

        public async Task<List<ShelfBook>> Find(Expression<Func<ShelfBook, bool>> filter)
        {
            return await _repo.Find(filter);
        }

        public async Task<List<ShelfBook>> GetAll()
        {
            return await _repo.GetAll();
        }

        public async Task<ShelfBook> GetById(int id)
        {
            return await _repo.GetById(id);
        }

        public async Task Update(ShelfBook entity)
        {
            await _repo.Update(entity);
        }

        public async Task<IEnumerable<ShelfBook>> GetAllShelfBooks()
        {
            return await _repo.GetAll();
        }
    }
}

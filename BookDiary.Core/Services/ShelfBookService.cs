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

        public Task Add(Task entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ShelfBook>> Find(Expression<Func<ShelfBook, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Task<List<ShelfBook>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<ShelfBook> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(ShelfBook entity)
        {
            throw new NotImplementedException();
        }
    }
}

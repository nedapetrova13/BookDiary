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

        public Task Add(Shelf entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Shelf>> Find(Expression<Func<Shelf, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Task<List<Shelf>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Shelf> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(Shelf entity)
        {
            throw new NotImplementedException();
        }
    }
}

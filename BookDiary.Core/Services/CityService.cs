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
    public class CityService : ICityService
    {
        private readonly IRepository<City> _repo;
        public CityService(IRepository<City> repo)
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

        public Task<List<City>> Find(Expression<Func<City, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Task<List<City>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<City> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(City entity)
        {
            throw new NotImplementedException();
        }
    }
}

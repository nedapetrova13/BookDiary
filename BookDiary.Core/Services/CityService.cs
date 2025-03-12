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

        public async Task Add(City entity)
        {
            await _repo.Add(entity);
        }

        public async Task Delete(int id)
        {
            await _repo.Delete(id);
        }

        public async Task<List<City>> Find(Expression<Func<City, bool>> filter)
        {
            return await _repo.Find(filter);
        }

        public Task<City> Get(Expression<Func<City, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public  IQueryable<City> GetAll()
        {
            return _repo.GetAll();
        }

        public async Task<City> GetById(int id)
        {
            return await _repo.GetById(id);
        }

        public async Task Update(City entity)
        {
            await _repo.Update(entity);
        }

        
    }
}

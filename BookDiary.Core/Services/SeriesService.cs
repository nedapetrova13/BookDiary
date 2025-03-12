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
    public class SeriesService : ISeriesService
    {
        private readonly IRepository<Series> _repo;
        public SeriesService(IRepository<Series> repo)
        {
            this._repo = repo;
        }

        public async Task Add(Series entity)
        {
            await _repo.Add(entity);
        }

        public async Task Delete(int id)
        {
            await _repo.Delete(id);
        }

        public async Task<List<Series>> Find(Expression<Func<Series, bool>> filter)
        {
            return await _repo.Find(filter);
        }

        public async Task<Series> Get(Expression<Func<Series, bool>> filter)
        {
            return await _repo.Get(filter);
        }

        public IQueryable<Series> GetAll()
        {
            return  _repo.GetAll();
        }

        public async Task<Series> GetById(int id)
        {
            return await _repo.GetById(id);
        }

        public async Task Update(Series entity)
        {
            await _repo.Update(entity);
        }

        
    }
}

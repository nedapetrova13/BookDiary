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

        public Task Add(Task entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Series>> Find(Expression<Func<Series, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Task<List<Series>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Series> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(Series entity)
        {
            throw new NotImplementedException();
        }
    }
}

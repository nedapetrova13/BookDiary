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
        public void Add(Task entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Series> Find(Expression<Func<Series, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public List<Series> GetAll()
        {
            throw new NotImplementedException();
        }

        public Series GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Series entity)
        {
            throw new NotImplementedException();
        }
    }
}

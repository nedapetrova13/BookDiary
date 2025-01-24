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
    public class GenreService : IGenreService
    {
        private readonly IRepository<Genre> _repo;
        public GenreService(IRepository<Genre> repo)
        {
            this._repo = repo;
        }

        public Task Add(Genre entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Genre>> Find(Expression<Func<Genre, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Task<List<Genre>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Genre> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(Genre entity)
        {
            throw new NotImplementedException();
        }
    }
}

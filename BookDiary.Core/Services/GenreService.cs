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

        public async Task Add(Genre entity)
        {
           await _repo.Add(entity);
        }

        public async Task Delete(int id)
        {
            await _repo.Delete(id);
        }

        public async Task<List<Genre>> Find(Expression<Func<Genre, bool>> filter)
        {
            return await _repo.Find(filter);    
        }

        public Task<Genre> Get(Expression<Func<Genre, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Genre> GetAll()
        {
            return  _repo.GetAll();
        }

        public async Task<Genre> GetById(int id)
        {
            return await _repo.GetById(id);
        }

        public async Task Update(Genre entity)
        {
            await _repo.Update(entity); 
        }

        
    }
}

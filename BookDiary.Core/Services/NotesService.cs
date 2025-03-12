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
    public class NotesService : INotesService
    {
        private readonly IRepository<Notes> _repo;
        public NotesService(IRepository<Notes> repo)
        {
            this._repo = repo;
        }

        public async Task Add(Notes entity)
        {
            await _repo.Add(entity);
        }

        public async Task Delete(int id)
        {
            await _repo.Delete(id);
        }

        public async Task<List<Notes>> Find(Expression<Func<Notes, bool>> filter)
        {
            return await _repo.Find(filter);
        }

        public Task<Notes> Get(Expression<Func<Notes, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Notes> GetAll()
        {
            return _repo.GetAll();
        }

        public async Task<Notes> GetById(int id)
        {
            return await _repo.GetById(id);
        }

        public async Task Update(Notes entity)
        {
            await _repo.Update(entity);
        }

       
    }
}

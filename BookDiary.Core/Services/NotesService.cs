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

        public Task Add(Task entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Notes>> Find(Expression<Func<Notes, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Task<List<Notes>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Notes> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(Notes entity)
        {
            throw new NotImplementedException();
        }
    }
}

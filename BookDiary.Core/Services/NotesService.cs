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
        public NotesService()
        public void Add(Task entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Notes> Find(Expression<Func<Notes, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public List<Notes> GetAll()
        {
            throw new NotImplementedException();
        }

        public Notes GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Notes entity)
        {
            throw new NotImplementedException();
        }
    }
}

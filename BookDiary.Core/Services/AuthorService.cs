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
    public class AuthorService : IAuthorService
    {
        private readonly IRepository<Author> _repo;
        public AuthorService(IRepository<Author> repo)
        {
            this._repo= repo;
        }

        public Task Add(Task entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Author>> Find(Expression<Func<Author, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Task<List<Author>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Author> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(Author entity)
        {
            throw new NotImplementedException();
        }
    }
}

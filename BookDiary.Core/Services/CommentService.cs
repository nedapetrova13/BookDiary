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
    public class CommentService : ICommentService
    {
        private readonly IRepository<Comment> _repo;
        public CommentService(IRepository<Comment> repo)
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

        public Task<List<Comment>> Find(Expression<Func<Comment, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Task<List<Comment>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Comment> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(Comment entity)
        {
            throw new NotImplementedException();
        }
    }
}

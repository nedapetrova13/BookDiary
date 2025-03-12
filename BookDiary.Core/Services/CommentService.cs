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

        public async Task Add(Comment entity)
        {
            await _repo.Add(entity);
        }

        public async Task Delete(int id)
        {
            await _repo.Delete(id);
        }

        public async Task<List<Comment>> Find(Expression<Func<Comment, bool>> filter)
        {
            return await _repo.Find(filter);
        }

        public Task<Comment> Get(Expression<Func<Comment, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Comment> GetAll()
        {
            return  _repo.GetAll();
        }

        public async Task<Comment> GetById(int id)
        {
            return await _repo.GetById(id);
        }

        public async Task Update(Comment entity)
        {
            await _repo.Update(entity);
        }

       
    }
}

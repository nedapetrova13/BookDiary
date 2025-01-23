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
    public class CommentNewsService : ICommentNewsService
    {
        private readonly IRepository<CommentNews> _repo;
        public CommentNewsService(IRepository<CommentNews> repo)
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

        public Task<List<CommentNews>> Find(Expression<Func<CommentNews, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Task<List<CommentNews>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<CommentNews> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(CommentNews entity)
        {
            throw new NotImplementedException();
        }
    }
}

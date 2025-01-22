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
        public void Add(Task entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<CommentNews> Find(Expression<Func<CommentNews, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public List<CommentNews> GetAll()
        {
            throw new NotImplementedException();
        }

        public CommentNews GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(CommentNews entity)
        {
            throw new NotImplementedException();
        }
    }
}

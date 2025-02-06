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

        public async Task Add(CommentNews entity)
        {
           await _repo.Add(entity);
        }

        public async Task Delete(int id)
        {
            await _repo.Delete(id);
        }

        public async Task<List<CommentNews>> Find(Expression<Func<CommentNews, bool>> filter)
        {
            return await _repo.Find(filter);    
        }

        public  IQueryable<CommentNews> GetAll()
        {
           return  _repo.GetAll();
        }

        public async Task<CommentNews> GetById(int id)
        {
            return await GetById(id);   
        }

        public async Task Update(CommentNews entity)
        {
            await _repo.Update(entity);
        }

       
    }
}

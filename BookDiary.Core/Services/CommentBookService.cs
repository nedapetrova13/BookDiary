using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BookDiary.Core.IServices;
using BookDiary.DataAccess.Repository;
using BookDiary.Models;

namespace BookDiary.Core.Services
{
    public class CommentBookService : ICommentBookService
    {
        private readonly IRepository<CommentBook> _repo;
        public CommentBookService(IRepository<CommentBook> repo)
        {
            this._repo = repo;
        }

        public async Task Add(CommentBook entity)
        {
            await _repo.Add(entity);
        }

        public async Task Delete(int id)
        {
            await _repo.Delete(id);
        }

        public async Task DeleteBookComment(int bookid, int comid)
        {
             await _repo.DeleteMapping<CommentBook>(bt => bt.BookId == bookid && bt.CommentId == comid);

        }

        public async Task<List<CommentBook>> Find(Expression<Func<CommentBook, bool>> filter)
        {
            return await _repo.Find(filter);
        }

        public Task<CommentBook> Get(Expression<Func<CommentBook, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public IQueryable<CommentBook> GetAll()
        {
            return  _repo.GetAll();
        }

        public async Task<CommentBook> GetById(int id)
        {
            return await _repo.GetById(id);
        }

        public async Task Update(CommentBook entity)
        {
            await _repo.Update(entity);
        }

    }
}

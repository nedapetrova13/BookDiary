﻿using System;
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
    public class CommentBookService : ICommentBookService
    {
        private readonly IRepository<CommentBook> _repo;
        public CommentBookService(IRepository<CommentBook> repo)
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

        public Task<List<CommentBook>> Find(Expression<Func<CommentBook, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Task<List<CommentBook>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<CommentBook> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(CommentBook entity)
        {
            throw new NotImplementedException();
        }
    }
}

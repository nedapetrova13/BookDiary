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
    public class TagService : ITagService
    {
        private readonly IRepository<Tag> _repo;
        public TagService(IRepository<Tag> repo)
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

        public Task<List<Tag>> Find(Expression<Func<Tag, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Task<List<Tag>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Tag> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(Tag entity)
        {
            throw new NotImplementedException();
        }
    }
}

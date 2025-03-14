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
    public class QuestionService : IQuestionService
    {
        private readonly IRepository<Question> _repo;
        public QuestionService(IRepository<Question> repo)
        {
            this._repo = repo;
        }

        public async Task Add(Question entity)
        {
            await _repo.Add(entity);
        }

        public async Task Delete(int id)
        {
            await _repo.Delete(id);
        }

        public async Task<List<Question>> Find(Expression<Func<Question, bool>> filter)
        {
            return await _repo.Find(filter);    
        }

        public Task<Question> Get(Expression<Func<Question, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Question> GetAll()
        {
            return  _repo.GetAll();
        }

        public async Task<Question> GetById(int id)
        {
            return await _repo.GetById(id);
        }

        public async Task Update(Question entity)
        {
            await _repo.Update(entity);
        }

       
    }
}

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
    public class QuestionService : IQuestionService
    {
        private readonly IRepository<Question> _repo;
        public QuestionService(IRepository<Question> repo)
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

        public Task<List<Question>> Find(Expression<Func<Question, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Task<List<Question>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Question> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(Question entity)
        {
            throw new NotImplementedException();
        }
    }
}

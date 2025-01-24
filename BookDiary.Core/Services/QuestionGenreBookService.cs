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
    public class QuestionGenreBookService : IQuestionGenreBookService
    {
        private readonly IRepository<QuestionGenreBook> _repo;
        public QuestionGenreBookService(IRepository<QuestionGenreBook> repo)
        {
            this._repo = repo;  
        }

        public Task Add(QuestionGenreBook entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<QuestionGenreBook>> Find(Expression<Func<QuestionGenreBook, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Task<List<QuestionGenreBook>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<QuestionGenreBook> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(QuestionGenreBook entity)
        {
            throw new NotImplementedException();
        }
    }
}

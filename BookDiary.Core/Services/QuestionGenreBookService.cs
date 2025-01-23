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
        public void Add(Task entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<QuestionGenreBook> Find(Expression<Func<QuestionGenreBook, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public List<QuestionGenreBook> GetAll()
        {
            throw new NotImplementedException();
        }

        public QuestionGenreBook GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(QuestionGenreBook entity)
        {
            throw new NotImplementedException();
        }
    }
}

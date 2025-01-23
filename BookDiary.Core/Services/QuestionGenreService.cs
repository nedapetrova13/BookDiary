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
    public class QuestionGenreService : IQuestionGenreService
    {
        private readonly IRepository<QuestionGenre> _repo;
        public QuestionGenreService(IRepository<QuestionGenre> repo)
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

        public List<QuestionGenre> Find(Expression<Func<QuestionGenre, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public List<QuestionGenre> GetAll()
        {
            throw new NotImplementedException();
        }

        public QuestionGenre GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(QuestionGenre entity)
        {
            throw new NotImplementedException();
        }
    }
}

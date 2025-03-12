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

        public async Task Add(QuestionGenre entity)
        {
            await _repo.Add(entity);
        }

        public async Task Delete(int id)
        {
            await _repo.Delete(id);
        }

        public async Task<List<QuestionGenre>> Find(Expression<Func<QuestionGenre, bool>> filter)
        {
            return await _repo.Find(filter);
        }

        public Task<QuestionGenre> Get(Expression<Func<QuestionGenre, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public IQueryable<QuestionGenre> GetAll()
        {
            return  _repo.GetAll();
        }

        public async Task<QuestionGenre> GetById(int id)
        {
            return await _repo.GetById(id);
        }

        public async Task Update(QuestionGenre entity)
        {
            await _repo.Update(entity); 
        }

      
    }
}

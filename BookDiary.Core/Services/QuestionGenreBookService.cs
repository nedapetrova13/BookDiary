using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
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

        public async Task Add(QuestionGenreBook entity)
        {
            await _repo.Add(entity);
        }

        public async Task Delete(int id)
        {
            await _repo.Delete(id);
        }

        public async Task<List<QuestionGenreBook>> Find(Expression<Func<QuestionGenreBook, bool>> filter)
        {
            return await _repo.Find(filter);    
        }

        public async Task<List<QuestionGenreBook>> GetAll()
        {
            return await _repo.GetAll();
        }

        public async Task<QuestionGenreBook> GetById(int id)
        {
           return await _repo.GetById(id);
        }

        public async Task Update(QuestionGenreBook entity)
        {
           await _repo.Update(entity);  
        }

        public async Task<IEnumerable<QuestionGenreBook>> GetAllQuestionGenreBooks()
        {
            return await _repo.GetAll();
        }
    }
}

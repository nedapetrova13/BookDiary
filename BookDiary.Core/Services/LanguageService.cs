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
    public class LanguageService : ILanguageService
    {
        private readonly IRepository<Language> _repo;
        public LanguageService(IRepository<Language> repo)
        {
            this._repo = repo;
        }

        public async Task Add(Language entity)
        {

            await _repo.Add(entity);
        }

        public async Task Delete(int id)
        {
            await _repo.Delete(id);
        }

        public async Task<List<Language>> Find(Expression<Func<Language, bool>> filter)
        {
            return await _repo.Find(filter);
        }

        public IQueryable<Language> GetAll()
        {
            return _repo.GetAll();
        }

        public async Task<Language> GetById(int id)
        {
            return await _repo.GetById(id);
        }

        public async Task Update(Language entity)
        {
            await _repo.Update(entity);   
        }
       
    }
}

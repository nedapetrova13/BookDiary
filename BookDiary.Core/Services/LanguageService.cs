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

        public void Add(Task entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Language> Find(Expression<Func<Language, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public List<Language> GetAll()
        {
            throw new NotImplementedException();
        }

        public Language GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Language entity)
        {
            throw new NotImplementedException();
        }
    }
}

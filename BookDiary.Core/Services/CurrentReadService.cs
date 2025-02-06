using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BookDiary.Core.IServices;
using BookDiary.DataAccess.Repository;
using BookDiary.Models;

namespace BookDiary.Core.Services
{
    public class CurrentReadService : ICurrentReadService
    {
        private readonly IRepository<CurrentRead> _repo;
        public CurrentReadService(IRepository<CurrentRead> repo)
        {
            this._repo = repo;
        }

        public async Task Add(CurrentRead entity)
        {
            await _repo.Add(entity);
        }

        public async Task Delete(int id)
        {
            await _repo.Delete(id);
        }

        public async Task<List<CurrentRead>> Find(Expression<Func<CurrentRead, bool>> filter)
        {
            return await _repo.Find(filter);
        }

        public IQueryable<CurrentRead> GetAll()
        {
            return  _repo.GetAll();
        }

        public async  Task<CurrentRead> GetById(int id)
        {
            return await _repo.GetById(id);
        }

        public async Task Update(CurrentRead entity)
        {
           await _repo.Update(entity);
        }

       
    }
}

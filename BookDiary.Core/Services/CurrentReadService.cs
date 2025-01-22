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
    public class CurrentReadService : ICurrentReadService
    {
        private readonly IRepository<CurrentRead> _repo;
        public CurrentReadService(IRepository<CurrentRead> repo)
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

        public List<ICurrentReadService> Find(Expression<Func<ICurrentReadService, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public List<ICurrentReadService> GetAll()
        {
            throw new NotImplementedException();
        }

        public ICurrentReadService GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(ICurrentReadService entity)
        {
            throw new NotImplementedException();
        }
    }
}

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
    public class PublishingHouseService : IPublishingHouseService
    {
        private readonly IRepository<PublishingHouse> _repo;
        public PublishingHouseService(IRepository<PublishingHouse> repo)
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

        public Task<List<PublishingHouse>> Find(Expression<Func<PublishingHouse, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Task<List<PublishingHouse>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<PublishingHouse> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(PublishingHouse entity)
        {
            throw new NotImplementedException();
        }
    }
}

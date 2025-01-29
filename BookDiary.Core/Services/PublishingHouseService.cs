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

        public async Task Add(PublishingHouse entity)
        {
            await _repo.Add(entity);
        }

        public async Task Delete(int id)
        {
            await _repo.Delete(id);
        }

        public async Task<List<PublishingHouse>> Find(Expression<Func<PublishingHouse, bool>> filter)
        {
            return await _repo.Find(filter);
        }

        public async Task<List<PublishingHouse>> GetAll()
        {
            return await _repo.GetAll();
        }

        public async Task<PublishingHouse> GetById(int id)
        {
            return await _repo.GetById(id);
        }

        public async Task Update(PublishingHouse entity)
        {
            await _repo.Update(entity);
        }

        public async Task<IEnumerable<PublishingHouse>> GetAllPublishingHouses()
        {
            return await _repo.GetAll();
        }
    }
}

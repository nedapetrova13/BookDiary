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
    public class NewsService : INewsService
    {
        private readonly IRepository<News> _repo;
        public NewsService(IRepository<News> repo)
        {
            this._repo = repo;
        }

        public async Task Add(News entity)
        {
            await _repo.Add(entity);
        }

        public async Task Delete(int id)
        {
            await _repo.Delete(id);
        }

        public async Task<List<News>> Find(Expression<Func<News, bool>> filter)
        {
            return await _repo.Find(filter);
        }

        public async Task<List<News>> GetAll()
        {
            return await _repo.GetAll();
        }

        public async Task<News> GetById(int id)
        {
            return await _repo.GetById(id);
        }

        public async Task Update(News entity)
        {
            await _repo.Update(entity);
        }

        public async Task<IEnumerable<News>> GetAllNews()
        {
            return await _repo.GetAll();
        }

        public async Task<IEnumerable<News>> GetTop5Services()
        {
            var newsList = await _repo.GetAll(); // Await the list of news
            return newsList.OrderByDescending(n => n.Created).Take(3);       
         }
    }
}

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
    public class UserService : IUserService
    {
        private readonly IRepository<User> _repo;
        public UserService(IRepository<User> repo)
        {
            this._repo= repo;   
        }

        public async Task Add(User entity)
        {
            await _repo.Add(entity);
        }

        public async Task Delete(int id)
        {
            await _repo.Delete(id);
        }

        public async Task<List<User>> Find(Expression<Func<User, bool>> filter)
        {
            return await _repo.Find(filter);
        }

        public IQueryable<User> GetAll()
        {
            return  _repo.GetAll();
        }

        public async Task<User> GetById(int id)
        {
            return await _repo.GetById(id);
        }

        public async Task Update(User entity)
        {
            await _repo.Update(entity);
        }

       /* public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _repo.GetAll();
        }*/
    }
}

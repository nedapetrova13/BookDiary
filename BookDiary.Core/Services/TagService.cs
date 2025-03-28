using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BookDiary.Core.IServices;
using BookDiary.Core.Validators;
using BookDiary.DataAccess.Repository;
using BookDiary.Models;

namespace BookDiary.Core.Services
{
    public class TagService : ITagService
    {
        private readonly IRepository<Tag> _repo;
        public TagService(IRepository<Tag> repo)
        {
            this._repo = repo;
        }

        public async Task Add(Tag entity)
        {
            await _repo.Add(entity);
        }

        public async Task Delete(int id)
        {
            await _repo.Delete(id);
        }

        public async Task<List<Tag>> Find(Expression<Func<Tag, bool>> filter)
        {
            return await _repo.Find(filter);
        }

        public async Task<Tag> Get(Expression<Func<Tag, bool>> filter)
        {
            return await _repo.Get(filter);
        }

        /* public  IQueryable<Tag> GetAllTags()
{
    return _repo.GetAll();
}*/
        public  IQueryable<Tag> GetAll()
        {
            return  _repo.GetAll();
        }

        public async Task<Tag> GetById(int id)
        {
            return await _repo.GetById(id);
        }

        public async Task Update(Tag entity)
        {
            await _repo.Update(entity);
        }

        
        private bool ValidateTag(Tag tag)
        {
            if (!TagValidator.ValidateInput(tag.Name))
            {
                return false;   
            }
            else
            {
                return true;
            }
        }
        
    }
}

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
    public class AuthorService : IAuthorService
    {
        private readonly IRepository<Author> _repo;
        public AuthorService(IRepository<Author> repo)
        {
            this._repo= repo;
        }

        public async Task Add(Author entity)
        {
            await _repo.Add(entity);

        }

        public async Task Delete(int id)
        {
            await _repo.Delete(id);
        }

        public async Task<List<Author>> Find(Expression<Func<Author, bool>> filter)
        {
            return await _repo.Find(filter);
        }

        public IQueryable<Author> GetAll()
        {
            return  _repo.GetAll();
        }

        public async Task<Author> GetById(int id)
        {
            return await _repo.GetById(id);
        }

        public Task Update(Author entity)
        {
            return _repo.Update(entity);
        }
        /*public IQueryable<Author> GetAllAuthors()
        {
            return _repo.GetAll();
        }
*/
        private bool ValidateAuthor(Author author)
        {
            if (!AuthorValidator.ValidateInput(author.Name, author.Email, author.ProfilePictureURL, author.Bio, author.WebSiteLink))
            {
                return false;
            }
            else if (!AuthorValidator.AuthorExists(author.Id))
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

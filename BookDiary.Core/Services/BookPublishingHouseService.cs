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
    public class BookPublishingHouseService : IBookPublishingHouseService
    {
        private readonly IRepository<BookPublishingHouse> _repo;
        public BookPublishingHouseService(IRepository<BookPublishingHouse> repo)
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

        public List<BookPublishingHouse> Find(Expression<Func<BookPublishingHouse, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public List<BookPublishingHouse> GetAll()
        {
            throw new NotImplementedException();
        }

        public BookPublishingHouse GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(BookPublishingHouse entity)
        {
            throw new NotImplementedException();
        }
    }
}

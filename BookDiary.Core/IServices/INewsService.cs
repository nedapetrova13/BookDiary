using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookDiary.Models;

namespace BookDiary.Core.IServices
{
    public interface INewsService:IService<News>
    {
        Task<IEnumerable<News>> GetAllNews();
        Task<IEnumerable<News>> GetTop5Services();


    }
}

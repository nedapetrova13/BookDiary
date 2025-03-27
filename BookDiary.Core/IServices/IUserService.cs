using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookDiary.Models;

namespace BookDiary.Core.IServices
{
    public interface IUserService:IService<User>
    {
        // Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetByIdUser(string id);
        Task DeleteUser(string id);
    }
}

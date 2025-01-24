using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookDiary.DataAccess.Repository;
using BookDiary.Models;

namespace BookDiary.Core.Validators
{
    public class TagValidator
    {
        private static IRepository<Tag> _repo;
        public static bool TagExists(int id)
        {
            if(_repo.Get(id) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static bool ValidateInput(string name)
        {
            if(name.Length==0|| name.Length > 50)
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

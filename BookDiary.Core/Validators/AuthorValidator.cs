using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookDiary.DataAccess.Repository;
using BookDiary.Models;

namespace BookDiary.Core.Validators
{
    public static class AuthorValidator
    {
        private static IRepository<Author> _repo;
        public static bool AuthorExists(int id)
        {
            if (_repo.Get(id) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static bool ValidateInput(string name, string email, string imageurl, string bio, string websitelink)
        {
            if(name.Length==0 || name.Length > 50 || email.Length==0 || email.Length>50 || imageurl.Length==0 || bio.Length==0 || websitelink.Length==0)
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

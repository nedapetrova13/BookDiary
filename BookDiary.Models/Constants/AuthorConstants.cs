using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.Json;

namespace BookDiary.Models.Constants
{
    public static class AuthorConstants
    {
        public const int AuthorNameMaxLength = 50;
        public const int AuthorNameMinLength = 2;

        public const int AuthorEmailMaxLength = 50;
        public const int AuthorEmailMinLength = 8;

        public const int AuthorBioMaxLength = 1000;
        public const int AuthorBioMinLength = 20;
    }
}

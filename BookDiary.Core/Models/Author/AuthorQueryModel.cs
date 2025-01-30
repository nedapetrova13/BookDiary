using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookDiary.Models;

namespace BookDiary.Core.Models.Author
{
    public class AuthorQueryModel
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required]

        public string Email { get; set; } = null!;
        [Required]

        public DateTime BirthDate { get; set; }
        [Required]

        public string ProfilePictureURL { get; set; } = null!;
        [Required]

        public string? Gender { get; set; }
        [Required]

        public string Bio { get; set; } = null!;
        [Required]

        public string WebSiteLink { get; set; }=null!;

    }
}

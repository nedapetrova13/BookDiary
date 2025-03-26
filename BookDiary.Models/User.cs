using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookDiary.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace BookDiary.Models
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "Името е задължително")]
        public string Name { get; set; }
        public DateTime Birthdate { get; set; }
        [EnumDataType(typeof(GenderEnum))]
        public string? Gender { get; set; }
       
        public ICollection<Shelf>? Shelves { get; set; }   
        public string? ProfilePictureURL { get; set; }   
        public string? Bio { get; set; }
        public ICollection<CurrentRead>? CurrentReads { get; set; }
        public ICollection<Notes>? Notes { get; set; }
        public ICollection<Comment>? MyComments { get; set; }

    }
}

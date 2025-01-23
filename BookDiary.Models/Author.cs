using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using BookDiary.Models.Enums;
using static BookDiary.Models.Constants.AuthorConstants;


namespace BookDiary.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(AuthorNameMaxLength)]
        public string Name { get; set; }
        public ICollection<Book> Books { get; set; }
        [ForeignKey(nameof(City))]
        public int CityId { get; set; } 
        public City City { get; set; }
        public DateTime BirthDate { get; set; }
        [Required]
        [MaxLength(AuthorEmailMaxLength)]
        public string Email {  get; set; }
        public string ProfilePictureURL { get; set; }
        [EnumDataType(typeof(GenderEnum))]
        public string? Gender { get; set; }
        public string Bio {  get; set; }
        public string WebSiteLink { get; set; }
        public ICollection<Series> Series { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDiary.Models
{
    public class CurrentRead
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "заядължително поле")]
        [ForeignKey(nameof(Book))]

        public int BookId { get; set; }
        public Book Book { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Страниците трябва да са положително число")]
        public int CurrentPage { get; set; }
        [Required(ErrorMessage = "заядължително поле")]

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public User User { get; set; }

    }
}

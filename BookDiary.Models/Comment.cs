using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDiary.Models
{
    public class Comment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Key]
        public int Id {  get; set; }

        [Required(ErrorMessage = "Полето е задължително")]

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public User User { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        public string Content {  get; set; }

        [Required(ErrorMessage = "Оценката е задължителна")]
        [Range(1, 5, ErrorMessage = "Оценката трябва да е положително число")]

        public int Rating { get; set; } = 0!;

        [Required(ErrorMessage = "Полето е задължително")]

        [ForeignKey(nameof(Book))]
        public int BookId { get; set; }
        public Book Book { get; set; }
      
    }
}

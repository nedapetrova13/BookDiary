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
        [Key]
        public int Id {  get; set; }
   
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public User User { get; set; }
        [Required(ErrorMessage = "Полето е задължително")]

        public string Content {  get; set; }
        [Required(ErrorMessage = "Оценката е задължителна")]

        public int Rating { get; set; }
         [ForeignKey(nameof(Book))]
        public int BookId { get; set; }
        public Book Book { get; set; }
      
    }
}

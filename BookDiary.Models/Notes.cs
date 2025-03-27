using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDiary.Models
{
    public class Notes
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public User User { get; set; }
        [Required(ErrorMessage = "Главата е задължителна")]
        public int BookChapter {  get; set; }
        [Required(ErrorMessage = "Полето е задължително")]
        public string NoteContent { get; set; }
        [Required(ErrorMessage = "Името е заядължително")]
        public string Title { get; set; }
        [ForeignKey(nameof(Book))]
        public int BookId {  get; set; }
        public Book Book { get; set; }  

    }
}

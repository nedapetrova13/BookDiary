using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDiary.Models
{
    public class QuestionGenreBook
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(QuestionGenre))]
        public int QuestionId { get; set; }
        public int GenreId { get; set; }
        public QuestionGenre QuestionGenre { get; set; }
        [ForeignKey(nameof(Book))]
        public int BookId { get; set; }
        public Book Book { get; set; }
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public User User { get; set; }
        public int Rating { get;set; }
    }
}

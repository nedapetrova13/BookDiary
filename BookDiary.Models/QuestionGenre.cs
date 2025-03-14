using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDiary.Models
{
    public class QuestionGenre
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(Genre))]
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
        [ForeignKey(nameof(Question))]
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public ICollection<QuestionGenreBook> QuestionGenreBooks { get; set; }

    }
}

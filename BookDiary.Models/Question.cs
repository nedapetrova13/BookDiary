using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDiary.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Заядължително поле")]
        public string Content { get; set; }
        public ICollection<QuestionGenre> QuestionGenres { get; set; }

    }
}

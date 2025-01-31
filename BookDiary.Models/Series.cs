using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDiary.Models
{
    public class Series
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Името е задължително")]
        public string Title {  get; set; }  
        public ICollection<Book> Books { get; set; }
        public string Description { get; set; } 
    }
}

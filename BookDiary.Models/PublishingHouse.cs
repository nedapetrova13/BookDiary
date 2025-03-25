using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDiary.Models
{
    public class PublishingHouse
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Името е заядължително")]
        public string Name { get; set; }
        public int YearFounded { get; set; }
        public ICollection<Book> Books { get; set; }  

    }
}

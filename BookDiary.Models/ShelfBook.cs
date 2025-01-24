using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDiary.Models
{
    public class ShelfBook
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(Book))]
        public int BookId { get; set; }
        public Book Book { get; set; }
        [ForeignKey(nameof(Shelf))]
        public int ShelfId { get; set; }
        public Shelf Shelf { get; set; }

    }
}

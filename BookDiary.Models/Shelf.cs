using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDiary.Models
{
    public class Shelf
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }    
        public ICollection<ShelfBook> ShelfBooks { get; set; }
        public string Description { get; set; }
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}

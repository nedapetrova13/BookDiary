using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDiary.Models
{
    public class BookPublishingHouse
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(Book))]
        public int BookId {  get; set; }
        public Book Book { get; set; }
        [ForeignKey(nameof(PublishingHouse))]
        public int PublishingHouseId { get; set; }
        public PublishingHouse PublishingHouse { get; set; }
        [ForeignKey(nameof(Language))]
        public int LanguageId { get; set; }
        public Language Language { get; set; }
        public DateTime PublishingDate { get; set; }
    }
}

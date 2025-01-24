using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDiary.Models
{
    public class CommentNews
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(Comment))]
        public int CommentId { get; set; }
        public Comment Comment { get; set; }
        [ForeignKey(nameof(News))]
        public int NewsId { get; set; }
        public News News { get; set; }
    }
}

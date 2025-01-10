using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDiary.Models
{
    public class Comment
    {
        [Key]
        public int Id {  get; set; }
        public string Title {  get; set; }
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User User { get; set; }
        public string Content {  get; set; }
        public ICollection<CommentBook> CommentBooks { get; set; }  
        public ICollection<CommentNews> CommentNews { get; set; }
    }
}

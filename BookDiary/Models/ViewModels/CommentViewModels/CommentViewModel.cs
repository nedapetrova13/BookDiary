using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookDiary.Models.ViewModels.CommentViewModels
{
    public class CommentViewModel
    {
        public string UserId { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public int BookId { get; set; }

    }
}

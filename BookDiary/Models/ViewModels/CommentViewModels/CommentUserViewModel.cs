namespace BookDiary.Models.ViewModels.CommentViewModels
{
    public class CommentUserViewModel
    {
        public int Id { get; set; } 
        public string UserName { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public int BookId { get; set; }
    }
}

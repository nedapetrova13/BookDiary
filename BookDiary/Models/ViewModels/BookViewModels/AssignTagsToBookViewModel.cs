namespace BookDiary.Models.ViewModels.BookViewModels
{
    public class AssignTagsToBookViewModel
    {
        public int BookId { get; set; }

        public List<int> SelectedTagIds { get; set; } = new List<int>();

        public List<BookDiary.Models.Tag> TagList { get; set; } = new List<BookDiary.Models.Tag>();
    }
}

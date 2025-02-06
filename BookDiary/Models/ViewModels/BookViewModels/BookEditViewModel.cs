namespace BookDiary.Models.ViewModels.BookViewModels
{
    public class BookEditViewModel
    {
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public int GenreId { get; set; }
        public List<string> SelectedTags { get; set; } = new List<string>();
    }

}

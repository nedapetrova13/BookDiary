using BookDiary.Models.ViewModels.BookViewModels;

namespace BookDiary.Models.ViewModels.ShelfViewModels
{
    public class ShelfInfoViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<BookSeriesViewModel> Books { get; set; }
        public Book? SelectedBook { get; set; }
    }
}

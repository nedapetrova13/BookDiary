using BookDiary.Models.ViewModels.BookViewModels;

namespace BookDiary.Models.ViewModels.SeriesViewModels
{
    public class SeriesAdminViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<BookSeriesViewModel> Books { get; set; }
    }
}

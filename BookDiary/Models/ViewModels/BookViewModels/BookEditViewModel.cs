namespace BookDiary.Models.ViewModels.BookViewModels
{
    public class BookEditViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public int GenreId { get; set; }
        public int? SeriesId { get; set; }
        public string CoverImageURL { get; set; }
        public string BookFormat { get; set; }
        public int BookPages { get; set; }
        public int Chapters { get; set; }
        public List<string>? SelectedTags { get; set; } = new List<string>();
        public List<BookDiary.Models.PublishingHouse> PHList { get; set; } = new List<BookDiary.Models.PublishingHouse>();

        public string? PublishingHouse { get; set; }
        public List<BookDiary.Models.Language> LanguageList { get; set; } = new List<BookDiary.Models.Language>();

        public string? Language { get; set; }
    }

}

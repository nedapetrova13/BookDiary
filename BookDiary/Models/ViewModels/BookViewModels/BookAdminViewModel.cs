namespace BookDiary.Models.ViewModels.BookViewModels
{
    public class BookAdminViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AuthorName { get; set; }
        public string GenreName { get; set; }
        public string? SeriesName { get; set; }
        public int SeriesId { get; set; }
        public string CoverImageURL { get; set; }
        public string BookFormat { get; set; }
        public int BookPages { get; set; }
        public int Chapters { get; set; }
        public string? LanguageName { get; set; }
        public string? PublishingHouseName { get; set; }
        public List<string>? SelectedTags { get; set; } = new List<string>();
    }
}

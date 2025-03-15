namespace BookDiary.Models.ViewModels.BookViewModels
{
    public class BookCreateViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public int GenreId { get; set; }
        public int? SeriesId { get; set; }
        public string CoverImageURL { get; set; }
        public int BookPages {  get; set; }
        public int Chapters {  get; set; }
        public int LanguageId { get; set; }
        public int PublishingHouseId { get; set; }
    }
}

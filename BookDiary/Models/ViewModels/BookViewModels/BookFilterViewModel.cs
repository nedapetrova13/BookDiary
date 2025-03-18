using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookDiary.Models.ViewModels.BookViewModels
{
    public class BookFilterViewModel
    {
        public int BookId { get; set; }
        public int? GenreId { get; set; }
        public int? AuthorId { get; set; }
        public int? TagId { get; set; }
        public int? PublishingHouseId { get; set; }
        public int? LanguageId { get; set; }
        public int? PageMinCount { get; set; }
        public int? PageMaxCount { get; set; }
        public SelectList Genres { get; set; }
        public SelectList Authors { get; set; }
        public SelectList Tags { get; set; }
        public SelectList PublishingHouses { get; set; }
        public SelectList Languages { get; set; }
        public List<Book>? Books { get; set; }

    }
}

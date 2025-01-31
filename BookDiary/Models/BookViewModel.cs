using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookDiary.Models
{
    public class BookViewModel
    {
        public int? GenreId { get; set; }
        public int? AuthorId { get; set; }
        public int? PageMinCount {  get; set; }
        public int? PageMaxCount { get; set; }
        public SelectList Genres { get; set; }
        public SelectList Authors { get; set; }
        public List<Book>? Books { get; set; }
    }
}

using BookDiary.Models.ViewModels.BookViewModels;

namespace BookDiary.Models.ViewModels.AuthorViewModels
{
    public class AuthorAdminViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string ProfilePictureURL { get; set; }
        public string Bio { get; set; }
        public string WebSiteLink { get; set; }
        public List<BookSeriesViewModel> Books { get; set; }
    }
}

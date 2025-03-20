using BookDiary.Models.ViewModels.BookViewModels;

namespace BookDiary.Models.ViewModels.ProfileViewModels
{
    public class EditProfileViewModel
    {
        public EditProfileViewModel()
        {
            // Initialize to avoid null reference
            FavouriteBook = new BookSeriesViewModel();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public DateTime Birthdate { get; set; }
        public string ProfilePictureURL { get; set; }
        public BookSeriesViewModel FavouriteBook { get; set; }
    }
}

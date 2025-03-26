using BookDiary.Models.ViewModels.BookViewModels;

namespace BookDiary.Models.ViewModels.ProfileViewModels
{
    public class EditProfileViewModel
    {
        

        public string Id { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public DateTime Birthdate { get; set; }
        public string ProfilePictureURL { get; set; }
    }
}

namespace BookDiary.Models.ViewModels.AuthorViewModels
{
    public class AuthorEditViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string ProfilePictureURL { get; set; }
        public string? Gender { get; set; }
        public string Bio { get; set; }
        public string WebSiteLink { get; set; }
    }
}

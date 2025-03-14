namespace BookDiary.Models.ViewModels.ProfileViewModels
{
    public class AdminProfileViewModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? ProfilePictureUrl { get; set; }

        public string Email { get; set; } = null!;
    }
}

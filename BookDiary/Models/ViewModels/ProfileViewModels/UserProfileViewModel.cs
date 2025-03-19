using BookDiary.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BookDiary.Models.ViewModels.BookViewModels;

namespace BookDiary.Models.ViewModels.ProfileViewModels
{
    public class UserProfileViewModel
    {
        public string Id {get; set;}
        public string Name { get; set; }
        public DateTime Birthdate { get; set; }
        public BookSeriesViewModel? FavouriteBook { get; set; }
        public string? ProfilePictureURL { get; set; }
        public string? Bio { get; set; }
        public ICollection<CurrentRead>? CurrentReads { get; set; }


    }
}

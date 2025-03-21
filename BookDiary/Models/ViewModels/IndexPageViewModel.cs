using BookDiary.Models.ViewModels.CurrentReadViewModels;
using BookDiary.Models.ViewModels.NewsViewModels;
using BookDiary.Models.ViewModels.UserViewModels;

namespace BookDiary.Models.ViewModels
{
    public class IndexPageViewModel
    {
        public int BooksCount { get; set; }
        public int GenresCount { get; set; }
        public int AuthorsCount { get; set; }
        public List<NewsCreateViewModel> News { get; set; }
        public List<UserIndexViewModel> Users { get; set; }
        public List<CurrentReadIndexViewModel> CurrentReads { get; set; }
    }
}

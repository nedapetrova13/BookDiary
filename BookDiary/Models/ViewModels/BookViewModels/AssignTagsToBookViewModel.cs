namespace BookDiary.Models.ViewModels.BookViewModels
{
    public class AssignTagsToBookViewModel
    {
        public int BookId { get; set; }
        public List<Tag> SelectedTags { get; set; } = new List<Tag>(); // Already assigned tags
        public List<Tag> AvailableTags { get; set; } = new List<Tag>(); // Tags that can be added
        public List<int> SelectedTagIds { get; set; } = new List<int>(); // Tags to be assigned

    }
   

}

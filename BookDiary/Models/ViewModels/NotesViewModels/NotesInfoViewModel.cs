namespace BookDiary.Models.ViewModels.NotesViewModels
{
    public class NotesInfoViewModel
    {
        public int Id { get; set; }
        public int BookChapter { get; set; }
        public string NoteContent { get; set; }
        public string Title { get; set; }
        public int BookId { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookDiary.Models.ViewModels.NotesViewModels
{
    public class CreateNoteViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int BookChapter { get; set; }
        public string NoteContent { get; set; }
        public string Title { get; set; }
        public int BookId { get; set; }
        
    }
}

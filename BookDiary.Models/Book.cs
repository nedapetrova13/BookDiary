using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookDiary.Models.Enums;

namespace BookDiary.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Името е заядължително")]
        public string Title { get; set; }
        [ForeignKey(nameof(Author))]
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public double Rating { get; set; }
        [ForeignKey(nameof(Series))]
        public int? SeriesId { get; set; }
        public Series? Series { get; set; }
        public string CoverImageURL { get; set; }
        [ForeignKey(nameof(Genre))]
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
        public int BookPages { get; set; }
        [EnumDataType(typeof(BookFormatEnum))]
        public string? BookFormat { get; set; }
        [Required(ErrorMessage = "Описанието е заядължително")]
        public string Description {  get; set; }
        public ICollection<CommentBook> Comments { get; set; }
        public ICollection<CurrentRead> CurrentReads { get; set; }
        public ICollection<User> Users { get; set; }
        public int Chapters {  get; set; }
        public ICollection<Notes> Notes { get; set; }
        public ICollection<BookPublishingHouse> BookPublishingHouse { get; set; }
        public ICollection<BookTag> BookTags { get; set; }
        public ICollection<ShelfBook> ShelfBooks { get; set; }
        public ICollection<QuestionGenreBook> QuestionGenreBooks { get; set; }


    }
}

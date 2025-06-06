﻿using System;
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

        [Required(ErrorMessage = "Полето е задължително")]
        public string CoverImageURL { get; set; }

        [ForeignKey(nameof(Genre))]
        public int GenreId { get; set; }
        public Genre Genre { get; set; }

        [ForeignKey(nameof(PublishingHouse))]
        public int PublishingHouseId { get; set; }
        public PublishingHouse PublishingHouse { get; set; }

        [ForeignKey(nameof(Language))]
        public int LanguageId { get; set; }
        public Language Language { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        [Range(1, int.MaxValue, ErrorMessage = "Страниците трябва да са положително число")]
        public int BookPages { get; set; } = 0!;
        
        [Required(ErrorMessage = "Описанието е заядължително")]
        public string Description {  get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<CurrentRead> CurrentReads { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        [Range(1, int.MaxValue, ErrorMessage = "Главата трябва да е положително число")]
        public int Chapters { get; set; } = 0!;

        public ICollection<Notes> Notes { get; set; }

        public ICollection<BookTag> BookTags { get; set; }

        public ICollection<ShelfBook> ShelfBooks { get; set; }


    }
}

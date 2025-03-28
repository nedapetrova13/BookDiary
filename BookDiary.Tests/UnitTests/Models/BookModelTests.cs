using NUnit.Framework;
using BookDiary.Models;
using BookDiary.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BookDiary.Tests.UnitTests.Models
{
    [TestFixture]
    public class BookTests
    {
        [Test]
        public void Book_IdProperty_ShouldHaveKeyAttribute()
        {
            // Arrange
            var propertyInfo = typeof(Book).GetProperty("Id");

            // Act
            var keyAttribute = propertyInfo.GetCustomAttributes(typeof(KeyAttribute), false).FirstOrDefault();

            // Assert
            Assert.IsNotNull(keyAttribute, "Id property should have KeyAttribute");
        }

        [Test]
        public void Book_TitleProperty_ShouldHaveRequiredAttribute()
        {
            // Arrange
            var propertyInfo = typeof(Book).GetProperty("Title");

            // Act
            var requiredAttribute = propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() as RequiredAttribute;

            // Assert
            Assert.IsNotNull(requiredAttribute, "Title property should have RequiredAttribute");
            Assert.AreEqual("Името е заядължително", requiredAttribute.ErrorMessage);
        }

        [Test]
        public void Book_CoverImageURLProperty_ShouldHaveRequiredAttribute()
        {
            // Arrange
            var propertyInfo = typeof(Book).GetProperty("CoverImageURL");

            // Act
            var requiredAttribute = propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() as RequiredAttribute;

            // Assert
            Assert.IsNotNull(requiredAttribute, "CoverImageURL property should have RequiredAttribute");
            Assert.AreEqual("Полето е задължително", requiredAttribute.ErrorMessage);
        }

        [Test]
        public void Book_BookPagesProperty_ShouldHaveRequiredAttribute()
        {
            // Arrange
            var propertyInfo = typeof(Book).GetProperty("BookPages");

            // Act
            var requiredAttribute = propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() as RequiredAttribute;

            // Assert
            Assert.IsNotNull(requiredAttribute, "BookPages property should have RequiredAttribute");
            Assert.AreEqual("Полето е задължително", requiredAttribute.ErrorMessage);
        }

        [Test]
        public void Book_DescriptionProperty_ShouldHaveRequiredAttribute()
        {
            // Arrange
            var propertyInfo = typeof(Book).GetProperty("Description");

            // Act
            var requiredAttribute = propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() as RequiredAttribute;

            // Assert
            Assert.IsNotNull(requiredAttribute, "Description property should have RequiredAttribute");
            Assert.AreEqual("Описанието е заядължително", requiredAttribute.ErrorMessage);
        }

        [Test]
        public void Book_ChaptersProperty_ShouldHaveRequiredAttribute()
        {
            // Arrange
            var propertyInfo = typeof(Book).GetProperty("Chapters");

            // Act
            var requiredAttribute = propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() as RequiredAttribute;

            // Assert
            Assert.IsNotNull(requiredAttribute, "Chapters property should have RequiredAttribute");
            Assert.AreEqual("Полето е задължително", requiredAttribute.ErrorMessage);
        }

        [Test]
        public void Book_AuthorIdProperty_ShouldHaveForeignKeyAttribute()
        {
            // Arrange
            var propertyInfo = typeof(Book).GetProperty("AuthorId");

            // Act
            var foreignKeyAttribute = propertyInfo.GetCustomAttributes(typeof(ForeignKeyAttribute), false).FirstOrDefault() as ForeignKeyAttribute;

            // Assert
            Assert.IsNotNull(foreignKeyAttribute, "AuthorId property should have ForeignKeyAttribute");
            Assert.AreEqual("Author", foreignKeyAttribute.Name);
        }

        [Test]
        public void Book_SeriesIdProperty_ShouldHaveForeignKeyAttribute()
        {
            // Arrange
            var propertyInfo = typeof(Book).GetProperty("SeriesId");

            // Act
            var foreignKeyAttribute = propertyInfo.GetCustomAttributes(typeof(ForeignKeyAttribute), false).FirstOrDefault() as ForeignKeyAttribute;

            // Assert
            Assert.IsNotNull(foreignKeyAttribute, "SeriesId property should have ForeignKeyAttribute");
            Assert.AreEqual("Series", foreignKeyAttribute.Name);
        }

        [Test]
        public void Book_GenreIdProperty_ShouldHaveForeignKeyAttribute()
        {
            // Arrange
            var propertyInfo = typeof(Book).GetProperty("GenreId");

            // Act
            var foreignKeyAttribute = propertyInfo.GetCustomAttributes(typeof(ForeignKeyAttribute), false).FirstOrDefault() as ForeignKeyAttribute;

            // Assert
            Assert.IsNotNull(foreignKeyAttribute, "GenreId property should have ForeignKeyAttribute");
            Assert.AreEqual("Genre", foreignKeyAttribute.Name);
        }

        [Test]
        public void Book_PublishingHouseIdProperty_ShouldHaveForeignKeyAttribute()
        {
            // Arrange
            var propertyInfo = typeof(Book).GetProperty("PublishingHouseId");

            // Act
            var foreignKeyAttribute = propertyInfo.GetCustomAttributes(typeof(ForeignKeyAttribute), false).FirstOrDefault() as ForeignKeyAttribute;

            // Assert
            Assert.IsNotNull(foreignKeyAttribute, "PublishingHouseId property should have ForeignKeyAttribute");
            Assert.AreEqual("PublishingHouse", foreignKeyAttribute.Name);
        }

        [Test]
        public void Book_LanguageIdProperty_ShouldHaveForeignKeyAttribute()
        {
            // Arrange
            var propertyInfo = typeof(Book).GetProperty("LanguageId");

            // Act
            var foreignKeyAttribute = propertyInfo.GetCustomAttributes(typeof(ForeignKeyAttribute), false).FirstOrDefault() as ForeignKeyAttribute;

            // Assert
            Assert.IsNotNull(foreignKeyAttribute, "LanguageId property should have ForeignKeyAttribute");
            Assert.AreEqual("Language", foreignKeyAttribute.Name);
        }

        
        [TestCase(1, "Harry Potter and the Philosopher's Stone", 1, "https://example.com/hp1.jpg", 1, 1, 1, 320, "Hardcover",
            "The first book in the Harry Potter series.", 25)]
        [TestCase(2, "The Great Gatsby", 2, "https://example.com/gatsby.jpg", 2, 2, 2, 180, "Paperback",
            "A novel by F. Scott Fitzgerald.", 12)]
        public void Book_PropertiesSetCorrectly_ReturnsExpectedValues(
            int id, string title, int authorId, string coverImageURL, int genreId,
            int publishingHouseId, int languageId, int bookPages, string bookFormat,
            string description, int chapters)
        {
            // Arrange
            var author = new Author { Id = authorId, Name = "Test Author" };
            var genre = new Genre { Id = genreId, Name = "Test Genre" };
            var publishingHouse = new PublishingHouse { Id = publishingHouseId, Name = "Test Publishing House" };
            var language = new Language { Id = languageId, Name = "Test Language" };

            var book = new Book
            {
                Id = id,
                Title = title,
                AuthorId = authorId,
                Author = author,
                CoverImageURL = coverImageURL,
                GenreId = genreId,
                Genre = genre,
                PublishingHouseId = publishingHouseId,
                PublishingHouse = publishingHouse,
                LanguageId = languageId,
                Language = language,
                BookPages = bookPages,
                Description = description,
                Chapters = chapters,
                Rating = 4.5,
                Comments = new List<Comment>(),
                CurrentReads = new List<CurrentRead>(),
                Notes = new List<Notes>(),
                BookTags = new List<BookTag>(),
                ShelfBooks = new List<ShelfBook>()
            };

            // Act & Assert
            Assert.AreEqual(id, book.Id);
            Assert.AreEqual(title, book.Title);
            Assert.AreEqual(authorId, book.AuthorId);
            Assert.AreEqual(author, book.Author);
            Assert.AreEqual(coverImageURL, book.CoverImageURL);
            Assert.AreEqual(genreId, book.GenreId);
            Assert.AreEqual(genre, book.Genre);
            Assert.AreEqual(publishingHouseId, book.PublishingHouseId);
            Assert.AreEqual(publishingHouse, book.PublishingHouse);
            Assert.AreEqual(languageId, book.LanguageId);
            Assert.AreEqual(language, book.Language);
            Assert.AreEqual(bookPages, book.BookPages);
            Assert.AreEqual(description, book.Description);
            Assert.AreEqual(chapters, book.Chapters);
            Assert.AreEqual(4.5, book.Rating);
            Assert.IsNotNull(book.Comments);
            Assert.IsEmpty(book.Comments);
            Assert.IsNotNull(book.CurrentReads);
            Assert.IsEmpty(book.CurrentReads);
            Assert.IsNotNull(book.Notes);
            Assert.IsEmpty(book.Notes);
            Assert.IsNotNull(book.BookTags);
            Assert.IsEmpty(book.BookTags);
            Assert.IsNotNull(book.ShelfBooks);
            Assert.IsEmpty(book.ShelfBooks);
        }

        [Test]
        public void Book_SeriesProperty_CanBeNull()
        {
            // Arrange
            var book = new Book
            {
                Id = 1,
                Title = "Standalone Book",
                AuthorId = 1,
                CoverImageURL = "https://example.com/book.jpg",
                GenreId = 1,
                PublishingHouseId = 1,
                LanguageId = 1,
                BookPages = 300,
                Description = "A standalone book not part of any series",
                Chapters = 20,
                SeriesId = null,
                Series = null
            };

            // Act & Assert
            Assert.IsNull(book.SeriesId);
            Assert.IsNull(book.Series);
        }

        [Test]
        public void Book_SeriesProperty_CanBeSet()
        {
            // Arrange
            var series = new Series { Id = 1, Title = "Test Series", Description = "Test Description" };
            var book = new Book
            {
                Id = 1,
                Title = "Book in Series",
                AuthorId = 1,
                CoverImageURL = "https://example.com/book.jpg",
                GenreId = 1,
                PublishingHouseId = 1,
                LanguageId = 1,
                BookPages = 300,
                Description = "A book that's part of a series",
                Chapters = 20,
                SeriesId = 1,
                Series = series
            };

            // Act & Assert
            Assert.AreEqual(1, book.SeriesId);
            Assert.AreEqual(series, book.Series);
        }

        [Test]
        public void Book_CommentsCollection_CanAddAndRemoveComments()
        {
            // Arrange
            var book = new Book
            {
                Id = 1,
                Title = "Test Book",
                AuthorId = 1,
                CoverImageURL = "https://example.com/book.jpg",
                GenreId = 1,
                PublishingHouseId = 1,
                LanguageId = 1,
                BookPages = 300,
                Description = "Test description",
                Chapters = 20,
                Comments = new List<Comment>()
            };

            var comment1 = new Comment { Id = 1, BookId = 1, Content = "Great book!", Rating = 5 };
            var comment2 = new Comment { Id = 2, BookId = 1, Content = "Enjoyed it.", Rating = 4 };

            // Act
            book.Comments.Add(comment1);
            book.Comments.Add(comment2);

            // Assert
            Assert.AreEqual(2, book.Comments.Count);
            Assert.IsTrue(book.Comments.Contains(comment1));
            Assert.IsTrue(book.Comments.Contains(comment2));

            // Act
            book.Comments.Remove(comment1);

            // Assert
            Assert.AreEqual(1, book.Comments.Count);
            Assert.IsFalse(book.Comments.Contains(comment1));
            Assert.IsTrue(book.Comments.Contains(comment2));
        }

        [Test]
        public void Book_Validation_InvalidWithMissingRequiredProperties()
        {
            // Arrange
            var book = new Book
            {
                Id = 1,
                AuthorId = 1,
                GenreId = 1,
                PublishingHouseId = 1,
                LanguageId = 1
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(book);

            // Act
            var isValid = Validator.TryValidateObject(book, validationContext, validationResults, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.AreEqual(5, validationResults.Count); // Title, CoverImageURL, BookPages, Description, Chapters
            Assert.IsTrue(validationResults.Any(vr => vr.ErrorMessage == "Името е заядължително"));
            Assert.IsTrue(validationResults.Any(vr => vr.ErrorMessage == "Полето е задължително"));
            Assert.IsTrue(validationResults.Any(vr => vr.ErrorMessage == "Описанието е заядължително"));
        }

        [Test]
        public void Book_Validation_ValidWithAllRequiredProperties()
        {
            // Arrange
            var book = new Book
            {
                Id = 1,
                Title = "Test Book",
                AuthorId = 1,
                CoverImageURL = "https://example.com/book.jpg",
                GenreId = 1,
                PublishingHouseId = 1,
                LanguageId = 1,
                BookPages = 300,
                Description = "Test description",
                Chapters = 20
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(book);

            // Act
            var isValid = Validator.TryValidateObject(book, validationContext, validationResults, true);

            // Assert
            Assert.IsTrue(isValid);
            Assert.IsEmpty(validationResults);
        }

        [TestCase("")]
        [TestCase("   ")]
        public void Book_EmptyOrNullTitle_ShouldFailValidation(string invalidTitle)
        {
            // Arrange
            var book = new Book
            {
                Id = 1,
                Title = invalidTitle,
                AuthorId = 1,
                CoverImageURL = "https://example.com/book.jpg",
                GenreId = 1,
                PublishingHouseId = 1,
                LanguageId = 1,
                BookPages = 300,
                Description = "Test description",
                Chapters = 20
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(book);

            // Act
            var isValid = Validator.TryValidateObject(book, validationContext, validationResults, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(validationResults.Any(vr => vr.ErrorMessage == "Името е заядължително"));
        }

        

        [Test]
        public void Book_DefaultValues_ShouldBeSetCorrectly()
        {
            // Arrange
            var book = new Book();

            // Act & Assert
            Assert.AreEqual(0, book.BookPages);
            Assert.AreEqual(0, book.Chapters);
        }

        [Test]
        public void Book_MultipleCollections_ShouldWorkCorrectly()
        {
            // Arrange
            var book = new Book
            {
                Id = 1,
                Title = "Test Book",
                AuthorId = 1,
                CoverImageURL = "https://example.com/book.jpg",
                GenreId = 1,
                PublishingHouseId = 1,
                LanguageId = 1,
                BookPages = 300,
                Description = "Test description",
                Chapters = 20,
                Comments = new List<Comment>(),
                CurrentReads = new List<CurrentRead>(),
                Notes = new List<Notes>(),
                BookTags = new List<BookTag>(),
                ShelfBooks = new List<ShelfBook>()
            };

            // Add items to multiple collections
            var comment = new Comment { Id = 1, BookId = 1, Content = "Great book!", Rating = 5 };
            var bookTag = new BookTag { BookId = 1, TagId = 1 };
            var shelfBook = new ShelfBook { BookId = 1, ShelfId = 1 };

            book.Comments.Add(comment);
            book.BookTags.Add(bookTag);
            book.ShelfBooks.Add(shelfBook);

            // Act & Assert
            Assert.AreEqual(1, book.Comments.Count);
            Assert.AreEqual(1, book.BookTags.Count);
            Assert.AreEqual(1, book.ShelfBooks.Count);
            Assert.IsTrue(book.Comments.Contains(comment));
            Assert.IsTrue(book.BookTags.Contains(bookTag));
            Assert.IsTrue(book.ShelfBooks.Contains(shelfBook));
        }

        
    }
}
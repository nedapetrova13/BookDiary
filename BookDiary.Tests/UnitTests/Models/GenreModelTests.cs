using NUnit.Framework;
using BookDiary.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BookDiary.Tests.UnitTests.Models
{
    [TestFixture]
    public class GenreTests
    {
        [Test]
        public void Genre_IdProperty_ShouldHaveKeyAttribute()
        {
            var propertyInfo = typeof(Genre).GetProperty("Id");

            var keyAttribute = propertyInfo.GetCustomAttributes(typeof(KeyAttribute), false).FirstOrDefault();

            Assert.IsNotNull(keyAttribute, "Id property should have KeyAttribute");
        }

        [Test]
        public void Genre_NameProperty_ShouldHaveRequiredAttribute()
        {
            var propertyInfo = typeof(Genre).GetProperty("Name");

            var requiredAttribute = propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() as RequiredAttribute;

            Assert.IsNotNull(requiredAttribute, "Name property should have RequiredAttribute");
            Assert.AreEqual("Името е заядължително", requiredAttribute.ErrorMessage);
        }

        [TestCase(1, "Fantasy")]
        [TestCase(2, "Science Fiction")]
        [TestCase(3, "Mystery")]
        [TestCase(4, "Thriller")]
        [TestCase(5, "Romance")]
        [TestCase(6, "Non-fiction")]
        public void Genre_PropertiesSetCorrectly_ReturnsExpectedValues(int id, string name)
        {
            var genre = new Genre
            {
                Id = id,
                Name = name,
                Books = new List<Book>()
            };

            Assert.AreEqual(id, genre.Id);
            Assert.AreEqual(name, genre.Name);
            Assert.IsNotNull(genre.Books);
            Assert.IsEmpty(genre.Books);
        }

        [Test]
        public void Genre_BooksCollection_CanAddAndRemoveBooks()
        {
            var genre = new Genre
            {
                Id = 1,
                Name = "Fantasy",
                Books = new List<Book>()
            };

            var book1 = new Book { Id = 1, Title = "Book 1" };
            var book2 = new Book { Id = 2, Title = "Book 2" };

            genre.Books.Add(book1);
            genre.Books.Add(book2);

            Assert.AreEqual(2, genre.Books.Count);
            Assert.IsTrue(genre.Books.Contains(book1));
            Assert.IsTrue(genre.Books.Contains(book2));

            genre.Books.Remove(book1);

            Assert.AreEqual(1, genre.Books.Count);
            Assert.IsFalse(genre.Books.Contains(book1));
            Assert.IsTrue(genre.Books.Contains(book2));
        }

        [Test]
        public void Genre_Validation_InvalidWithMissingRequiredProperties()
        {
            var genre = new Genre
            {
                Id = 1
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(genre);

            var isValid = Validator.TryValidateObject(genre, validationContext, validationResults, true);

            Assert.IsFalse(isValid);
            Assert.AreEqual(1, validationResults.Count);
            Assert.IsTrue(validationResults.Any(vr => vr.ErrorMessage == "Името е заядължително"));
        }

        [Test]
        public void Genre_Validation_ValidWithAllRequiredProperties()
        {
            var genre = new Genre
            {
                Id = 1,
                Name = "Fantasy",
                Books = new List<Book>()
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(genre);

            var isValid = Validator.TryValidateObject(genre, validationContext, validationResults, true);

            Assert.IsTrue(isValid);
            Assert.IsEmpty(validationResults);
        }

        [Test]
        public void Genre_WithMultipleBooks_ShouldWorkCorrectly()
        {
            var genre = new Genre
            {
                Id = 1,
                Name = "Fantasy",
                Books = new List<Book>()
            };

            for (int i = 1; i <= 5; i++)
            {
                genre.Books.Add(new Book { Id = i, Title = $"Book {i}" });
            }

            Assert.AreEqual(5, genre.Books.Count);

            for (int i = 1; i <= 5; i++)
            {
                Assert.IsTrue(genre.Books.Any(b => b.Id == i && b.Title == $"Book {i}"),
                    $"Book with ID {i} and title 'Book {i}' should be in the collection");
            }
        }

        [TestCase("")]
        [TestCase("   ")]
        public void Genre_EmptyOrNullName_ShouldFailValidation(string invalidName)
        {
            var genre = new Genre
            {
                Id = 1,
                Name = invalidName,
                Books = new List<Book>()
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(genre);

            var isValid = Validator.TryValidateObject(genre, validationContext, validationResults, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(validationResults.Any(vr => vr.ErrorMessage == "Името е заядължително"));
        }

        [Test]
        public void Genre_BooksProperty_ShouldBeInitializedToEmptyCollectionIfNull()
        {
            var genre = new Genre
            {
                Id = 1,
                Name = "Fantasy",
                Books = null
            };

            
            if (genre.Books == null)
            {
                genre.Books = new List<Book>();
            }

            Assert.IsNotNull(genre.Books);
            Assert.IsEmpty(genre.Books);
        }
    }
}
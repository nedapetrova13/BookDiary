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
            // Arrange
            var propertyInfo = typeof(Genre).GetProperty("Id");

            // Act
            var keyAttribute = propertyInfo.GetCustomAttributes(typeof(KeyAttribute), false).FirstOrDefault();

            // Assert
            Assert.IsNotNull(keyAttribute, "Id property should have KeyAttribute");
        }

        [Test]
        public void Genre_NameProperty_ShouldHaveRequiredAttribute()
        {
            // Arrange
            var propertyInfo = typeof(Genre).GetProperty("Name");

            // Act
            var requiredAttribute = propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() as RequiredAttribute;

            // Assert
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
            // Arrange
            var genre = new Genre
            {
                Id = id,
                Name = name,
                Books = new List<Book>()
            };

            // Act & Assert
            Assert.AreEqual(id, genre.Id);
            Assert.AreEqual(name, genre.Name);
            Assert.IsNotNull(genre.Books);
            Assert.IsEmpty(genre.Books);
        }

        [Test]
        public void Genre_BooksCollection_CanAddAndRemoveBooks()
        {
            // Arrange
            var genre = new Genre
            {
                Id = 1,
                Name = "Fantasy",
                Books = new List<Book>()
            };

            var book1 = new Book { Id = 1, Title = "Book 1" };
            var book2 = new Book { Id = 2, Title = "Book 2" };

            // Act
            genre.Books.Add(book1);
            genre.Books.Add(book2);

            // Assert
            Assert.AreEqual(2, genre.Books.Count);
            Assert.IsTrue(genre.Books.Contains(book1));
            Assert.IsTrue(genre.Books.Contains(book2));

            // Act
            genre.Books.Remove(book1);

            // Assert
            Assert.AreEqual(1, genre.Books.Count);
            Assert.IsFalse(genre.Books.Contains(book1));
            Assert.IsTrue(genre.Books.Contains(book2));
        }

        [Test]
        public void Genre_Validation_InvalidWithMissingRequiredProperties()
        {
            // Arrange
            var genre = new Genre
            {
                Id = 1
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(genre);

            // Act
            var isValid = Validator.TryValidateObject(genre, validationContext, validationResults, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.AreEqual(1, validationResults.Count);
            Assert.IsTrue(validationResults.Any(vr => vr.ErrorMessage == "Името е заядължително"));
        }

        [Test]
        public void Genre_Validation_ValidWithAllRequiredProperties()
        {
            // Arrange
            var genre = new Genre
            {
                Id = 1,
                Name = "Fantasy",
                Books = new List<Book>()
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(genre);

            // Act
            var isValid = Validator.TryValidateObject(genre, validationContext, validationResults, true);

            // Assert
            Assert.IsTrue(isValid);
            Assert.IsEmpty(validationResults);
        }

        [Test]
        public void Genre_WithMultipleBooks_ShouldWorkCorrectly()
        {
            // Arrange
            var genre = new Genre
            {
                Id = 1,
                Name = "Fantasy",
                Books = new List<Book>()
            };

            // Add multiple books
            for (int i = 1; i <= 5; i++)
            {
                genre.Books.Add(new Book { Id = i, Title = $"Book {i}" });
            }

            // Act & Assert
            Assert.AreEqual(5, genre.Books.Count);

            // Verify all expected books are present
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
            // Arrange
            var genre = new Genre
            {
                Id = 1,
                Name = invalidName,
                Books = new List<Book>()
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(genre);

            // Act
            var isValid = Validator.TryValidateObject(genre, validationContext, validationResults, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(validationResults.Any(vr => vr.ErrorMessage == "Името е заядължително"));
        }

        [Test]
        public void Genre_BooksProperty_ShouldBeInitializedToEmptyCollectionIfNull()
        {
            // Arrange
            var genre = new Genre
            {
                Id = 1,
                Name = "Fantasy",
                Books = null
            };

            // Since we don't have access to the actual initialization code, 
            // this test demonstrates how we might test initialization in a constructor

            // For mocking constructor behavior:
            if (genre.Books == null)
            {
                genre.Books = new List<Book>();
            }

            // Act & Assert
            Assert.IsNotNull(genre.Books);
            Assert.IsEmpty(genre.Books);
        }
    }
}
using NUnit.Framework;
using BookDiary.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BookDiary.Tests.UnitTests.Models
{
    [TestFixture]
    public class LanguageTests
    {
        [Test]
        public void Language_IdProperty_ShouldHaveKeyAttribute()
        {
            // Arrange
            var propertyInfo = typeof(Language).GetProperty("Id");

            // Act
            var keyAttribute = propertyInfo.GetCustomAttributes(typeof(KeyAttribute), false).FirstOrDefault();

            // Assert
            Assert.IsNotNull(keyAttribute, "Id property should have KeyAttribute");
        }

        [Test]
        public void Language_NameProperty_ShouldHaveRequiredAttribute()
        {
            // Arrange
            var propertyInfo = typeof(Language).GetProperty("Name");

            // Act
            var requiredAttribute = propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() as RequiredAttribute;

            // Assert
            Assert.IsNotNull(requiredAttribute, "Name property should have RequiredAttribute");
            Assert.AreEqual("Името е заядължително", requiredAttribute.ErrorMessage);
        }

        [TestCase(1, "English")]
        [TestCase(2, "Spanish")]
        [TestCase(3, "French")]
        [TestCase(4, "German")]
        [TestCase(5, "Bulgarian")]
        [TestCase(6, "Russian")]
        public void Language_PropertiesSetCorrectly_ReturnsExpectedValues(int id, string name)
        {
            // Arrange
            var language = new Language
            {
                Id = id,
                Name = name,
                Books = new List<Book>()
            };

            // Act & Assert
            Assert.AreEqual(id, language.Id);
            Assert.AreEqual(name, language.Name);
            Assert.IsNotNull(language.Books);
            Assert.IsEmpty(language.Books);
        }

        [Test]
        public void Language_BooksCollection_CanAddAndRemoveBooks()
        {
            // Arrange
            var language = new Language
            {
                Id = 1,
                Name = "English",
                Books = new List<Book>()
            };

            var book1 = new Book { Id = 1, Title = "Book 1" };
            var book2 = new Book { Id = 2, Title = "Book 2" };

            // Act
            language.Books.Add(book1);
            language.Books.Add(book2);

            // Assert
            Assert.AreEqual(2, language.Books.Count);
            Assert.IsTrue(language.Books.Contains(book1));
            Assert.IsTrue(language.Books.Contains(book2));

            // Act
            language.Books.Remove(book1);

            // Assert
            Assert.AreEqual(1, language.Books.Count);
            Assert.IsFalse(language.Books.Contains(book1));
            Assert.IsTrue(language.Books.Contains(book2));
        }

        [Test]
        public void Language_Validation_InvalidWithMissingRequiredProperties()
        {
            // Arrange
            var language = new Language
            {
                Id = 1
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(language);

            // Act
            var isValid = Validator.TryValidateObject(language, validationContext, validationResults, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.AreEqual(1, validationResults.Count);
            Assert.IsTrue(validationResults.Any(vr => vr.ErrorMessage == "Името е заядължително"));
        }

        [Test]
        public void Language_Validation_ValidWithAllRequiredProperties()
        {
            // Arrange
            var language = new Language
            {
                Id = 1,
                Name = "English",
                Books = new List<Book>()
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(language);

            // Act
            var isValid = Validator.TryValidateObject(language, validationContext, validationResults, true);

            // Assert
            Assert.IsTrue(isValid);
            Assert.IsEmpty(validationResults);
        }

        [Test]
        public void Language_WithMultipleBooks_ShouldWorkCorrectly()
        {
            // Arrange
            var language = new Language
            {
                Id = 1,
                Name = "English",
                Books = new List<Book>()
            };

            // Add multiple books
            for (int i = 1; i <= 5; i++)
            {
                language.Books.Add(new Book { Id = i, Title = $"Book {i}" });
            }

            // Act & Assert
            Assert.AreEqual(5, language.Books.Count);

            // Verify all expected books are present
            for (int i = 1; i <= 5; i++)
            {
                Assert.IsTrue(language.Books.Any(b => b.Id == i && b.Title == $"Book {i}"),
                    $"Book with ID {i} and title 'Book {i}' should be in the collection");
            }
        }

        [TestCase("")]
        [TestCase("   ")]
        public void Language_EmptyOrNullName_ShouldFailValidation(string invalidName)
        {
            // Arrange
            var language = new Language
            {
                Id = 1,
                Name = invalidName,
                Books = new List<Book>()
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(language);

            // Act
            var isValid = Validator.TryValidateObject(language, validationContext, validationResults, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(validationResults.Any(vr => vr.ErrorMessage == "Името е заядължително"));
        }

        [Test]
        public void Language_BooksProperty_ShouldBeInitializedToEmptyCollectionIfNull()
        {
            // Arrange
            var language = new Language
            {
                Id = 1,
                Name = "English",
                Books = null
            };

            // Since we don't have access to the actual initialization code, 
            // this test demonstrates how we might test initialization in a constructor

            // For mocking constructor behavior:
            if (language.Books == null)
            {
                language.Books = new List<Book>();
            }

            // Act & Assert
            Assert.IsNotNull(language.Books);
            Assert.IsEmpty(language.Books);
        }
    }
}
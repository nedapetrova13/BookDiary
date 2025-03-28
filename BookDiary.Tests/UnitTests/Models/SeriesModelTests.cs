using NUnit.Framework;
using BookDiary.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BookDiary.Tests.UnitTests.Models
{
    [TestFixture]
    public class SeriesTests
    {
        [Test]
        public void Series_IdProperty_ShouldHaveKeyAttribute()
        {
            // Arrange
            var propertyInfo = typeof(Series).GetProperty("Id");

            // Act
            var keyAttribute = propertyInfo.GetCustomAttributes(typeof(KeyAttribute), false).FirstOrDefault();

            // Assert
            Assert.IsNotNull(keyAttribute, "Id property should have KeyAttribute");
        }

        [Test]
        public void Series_TitleProperty_ShouldHaveRequiredAttribute()
        {
            // Arrange
            var propertyInfo = typeof(Series).GetProperty("Title");

            // Act
            var requiredAttribute = propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() as RequiredAttribute;

            // Assert
            Assert.IsNotNull(requiredAttribute, "Title property should have RequiredAttribute");
            Assert.AreEqual("Името е задължително", requiredAttribute.ErrorMessage);
        }

        [Test]
        public void Series_DescriptionProperty_ShouldHaveRequiredAttribute()
        {
            // Arrange
            var propertyInfo = typeof(Series).GetProperty("Description");

            // Act
            var requiredAttribute = propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() as RequiredAttribute;

            // Assert
            Assert.IsNotNull(requiredAttribute, "Description property should have RequiredAttribute");
            Assert.AreEqual("Полето е задължително", requiredAttribute.ErrorMessage);
        }

        [TestCase(1, "Harry Potter Series", "A series about a young wizard")]
        [TestCase(2, "Lord of the Rings", "Epic fantasy novel by J.R.R. Tolkien")]
        [TestCase(3, "Game of Thrones", "A fantasy series by George R.R. Martin")]
        public void Series_PropertiesSetCorrectly_ReturnsExpectedValues(int id, string title, string description)
        {
            // Arrange
            var series = new Series
            {
                Id = id,
                Title = title,
                Description = description,
                Books = new List<Book>()
            };

            // Act & Assert
            Assert.AreEqual(id, series.Id);
            Assert.AreEqual(title, series.Title);
            Assert.AreEqual(description, series.Description);
            Assert.IsNotNull(series.Books);
            Assert.IsEmpty(series.Books);
        }

        [Test]
        public void Series_BooksCollection_CanAddAndRemoveBooks()
        {
            // Arrange
            var series = new Series
            {
                Id = 1,
                Title = "Test Series",
                Description = "Test Description",
                Books = new List<Book>()
            };

            var book1 = new Book { Id = 1, Title = "Book 1" };
            var book2 = new Book { Id = 2, Title = "Book 2" };

            // Act
            series.Books.Add(book1);
            series.Books.Add(book2);

            // Assert
            Assert.AreEqual(2, series.Books.Count);
            Assert.IsTrue(series.Books.Contains(book1));
            Assert.IsTrue(series.Books.Contains(book2));

            // Act
            series.Books.Remove(book1);

            // Assert
            Assert.AreEqual(1, series.Books.Count);
            Assert.IsFalse(series.Books.Contains(book1));
            Assert.IsTrue(series.Books.Contains(book2));
        }

        [Test]
        public void Series_Validation_InvalidWithMissingRequiredProperties()
        {
            // Arrange
            var series = new Series
            {
                Id = 1
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(series);

            // Act
            var isValid = Validator.TryValidateObject(series, validationContext, validationResults, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.AreEqual(2, validationResults.Count);
            Assert.IsTrue(validationResults.Any(vr => vr.ErrorMessage == "Името е задължително"));
            Assert.IsTrue(validationResults.Any(vr => vr.ErrorMessage == "Полето е задължително"));
        }

        [Test]
        public void Series_Validation_ValidWithAllRequiredProperties()
        {
            // Arrange
            var series = new Series
            {
                Id = 1,
                Title = "Test Series",
                Description = "Test Description",
                Books = new List<Book>()
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(series);

            // Act
            var isValid = Validator.TryValidateObject(series, validationContext, validationResults, true);

            // Assert
            Assert.IsTrue(isValid);
            Assert.IsEmpty(validationResults);
        }
    }
}
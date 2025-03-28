using NUnit.Framework;
using BookDiary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Moq;

namespace BookDiary.Tests.UnitTests.Models
{
    [TestFixture]
    public class CurrentReadTests
    {
        [Test]
        public void Constructor_Default_PropertiesInitializedCorrectly()
        {
            // Arrange & Act
            var currentRead = new CurrentRead();

            // Assert
            Assert.That(currentRead.Id, Is.EqualTo(0));
            Assert.That(currentRead.BookId, Is.EqualTo(0));
            Assert.That(currentRead.Book, Is.Null);
            Assert.That(currentRead.CurrentPage, Is.EqualTo(0));
            Assert.That(currentRead.UserId, Is.Null);
            Assert.That(currentRead.User, Is.Null);
        }

        [Test]
        [TestCase(1, 5, 10, "user123")]
        [TestCase(2, 10, 50, "user456")]
        [TestCase(3, 15, 100, "user789")]
        public void Properties_SetAndGet_ValuesMatchExpected(int id, int bookId, int currentPage, string userId)
        {
            // Arrange
            var currentRead = new CurrentRead
            {
                Id = id,
                BookId = bookId,
                CurrentPage = currentPage,
                UserId = userId
            };

            // Act & Assert
            Assert.That(currentRead.Id, Is.EqualTo(id));
            Assert.That(currentRead.BookId, Is.EqualTo(bookId));
            Assert.That(currentRead.CurrentPage, Is.EqualTo(currentPage));
            Assert.That(currentRead.UserId, Is.EqualTo(userId));
        }

        [Test]
        public void NavigationProperties_SetAndGet_ValuesMatchExpected()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "Test Book" };
            var user = new User { Id = "user123", UserName = "TestUser" };

            var currentRead = new CurrentRead
            {
                Book = book,
                User = user
            };

            // Act & Assert
            Assert.That(currentRead.Book, Is.SameAs(book));
            Assert.That(currentRead.User, Is.SameAs(user));
        }

        [Test]
        [TestCase(-1)]
        [TestCase(-100)]
        public void CurrentPage_NegativeValue_ThrowsValidationException(int invalidPage)
        {
            // Arrange
            var currentRead = new CurrentRead { CurrentPage = invalidPage };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(currentRead);

            // Act
            var isValid = Validator.TryValidateObject(currentRead, validationContext, validationResults, true);

            // Assert
            Assert.That(isValid, Is.False);
            // Note: This test assumes you'll add a Range validation attribute to CurrentPage
            // If you haven't added validation attributes, this test will fail
        }

        [Test]
        [TestCase(1, 0, "user123")]
        [TestCase(1, 1, " ")]
        [TestCase(1, 1, "")]
        public void RequiredProperties_MissingValues_ValidationFails(int bookId, int currentPage, string userId)
        {
            // Arrange
            var currentRead = new CurrentRead
            {
                BookId = bookId,
                CurrentPage = currentPage,
                UserId = userId
            };

            // Act & Assert
            var validationResults = ValidateModel(currentRead);

            // Note: This test assumes you'll add Required validation attributes
            // If you haven't added validation attributes, this test will fail
            Assert.That(validationResults.Count, Is.GreaterThan(0));
        }

        [Test]
        public void ForeignKeysAndNavigation_ManuallySetBoth_ValuesMatch()
        {
            // Arrange
            var book = new Book { Id = 5 };
            var user = new User { Id = "user123" };

            var currentRead = new CurrentRead
            {
                // Set both the navigation property and the foreign key
                Book = book,
                BookId = book.Id,
                User = user,
                UserId = user.Id
            };

            // Act & Assert
            Assert.That(currentRead.BookId, Is.EqualTo(book.Id));
            Assert.That(currentRead.UserId, Is.EqualTo(user.Id));
            Assert.That(currentRead.Book, Is.SameAs(book));
            Assert.That(currentRead.User, Is.SameAs(user));
        }

        // Helper method to validate model
        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model);
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            return validationResults;
        }
    }
}
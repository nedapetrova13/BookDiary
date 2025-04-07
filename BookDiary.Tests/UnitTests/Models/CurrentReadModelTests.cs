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
            var currentRead = new CurrentRead();

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
            var currentRead = new CurrentRead
            {
                Id = id,
                BookId = bookId,
                CurrentPage = currentPage,
                UserId = userId
            };

            Assert.That(currentRead.Id, Is.EqualTo(id));
            Assert.That(currentRead.BookId, Is.EqualTo(bookId));
            Assert.That(currentRead.CurrentPage, Is.EqualTo(currentPage));
            Assert.That(currentRead.UserId, Is.EqualTo(userId));
        }

        [Test]
        public void NavigationProperties_SetAndGet_ValuesMatchExpected()
        {
            var book = new Book { Id = 1, Title = "Test Book" };
            var user = new User { Id = "user123", UserName = "TestUser" };

            var currentRead = new CurrentRead
            {
                Book = book,
                User = user
            };

            Assert.That(currentRead.Book, Is.SameAs(book));
            Assert.That(currentRead.User, Is.SameAs(user));
        }

        [Test]
        [TestCase(-1)]
        [TestCase(-100)]
        public void CurrentPage_NegativeValue_ThrowsValidationException(int invalidPage)
        {
            var currentRead = new CurrentRead { CurrentPage = invalidPage };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(currentRead);

            var isValid = Validator.TryValidateObject(currentRead, validationContext, validationResults, true);

            Assert.That(isValid, Is.False);
        }

        [Test]
        [TestCase(1, 0, "user123")]
        [TestCase(1, 1, " ")]
        [TestCase(1, 1, "")]
        public void RequiredProperties_MissingValues_ValidationFails(int bookId, int currentPage, string userId)
        {
            var currentRead = new CurrentRead
            {
                BookId = bookId,
                CurrentPage = currentPage,
                UserId = userId
            };

            var validationResults = ValidateModel(currentRead);

            Assert.That(validationResults.Count, Is.GreaterThan(0));
        }

        [Test]
        public void ForeignKeysAndNavigation_ManuallySetBoth_ValuesMatch()
        {
            var book = new Book { Id = 5 };
            var user = new User { Id = "user123" };

            var currentRead = new CurrentRead
            {
                Book = book,
                BookId = book.Id,
                User = user,
                UserId = user.Id
            };

            Assert.That(currentRead.BookId, Is.EqualTo(book.Id));
            Assert.That(currentRead.UserId, Is.EqualTo(user.Id));
            Assert.That(currentRead.Book, Is.SameAs(book));
            Assert.That(currentRead.User, Is.SameAs(user));
        }

        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model);
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            return validationResults;
        }
    }
}
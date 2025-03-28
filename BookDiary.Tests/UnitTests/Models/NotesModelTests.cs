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
    public class NotesTests
    {
        [Test]
        public void Constructor_Default_PropertiesInitializedCorrectly()
        {
            // Arrange & Act
            var notes = new Notes();

            // Assert
            Assert.That(notes.Id, Is.EqualTo(0));
            Assert.That(notes.UserId, Is.Null);
            Assert.That(notes.User, Is.Null);
            Assert.That(notes.BookChapter, Is.EqualTo(0));
            Assert.That(notes.NoteContent, Is.Null);
            Assert.That(notes.Title, Is.Null);
            Assert.That(notes.BookId, Is.EqualTo(0));
            Assert.That(notes.Book, Is.Null);
        }

        [Test]
        [TestCase(1, "user123", 5, "This is a note", "Chapter 5 Notes", 10)]
        [TestCase(2, "user456", 10, "Another note content", "Important Points", 15)]
        [TestCase(3, "user789", 15, "Final thoughts", "Summary", 20)]
        public void Properties_SetAndGet_ValuesMatchExpected(
            int id, string userId, int bookChapter, string noteContent, string title, int bookId)
        {
            // Arrange
            var notes = new Notes
            {
                Id = id,
                UserId = userId,
                BookChapter = bookChapter,
                NoteContent = noteContent,
                Title = title,
                BookId = bookId
            };

            // Act & Assert
            Assert.That(notes.Id, Is.EqualTo(id));
            Assert.That(notes.UserId, Is.EqualTo(userId));
            Assert.That(notes.BookChapter, Is.EqualTo(bookChapter));
            Assert.That(notes.NoteContent, Is.EqualTo(noteContent));
            Assert.That(notes.Title, Is.EqualTo(title));
            Assert.That(notes.BookId, Is.EqualTo(bookId));
        }

        [Test]
        public void NavigationProperties_SetAndGet_ValuesMatchExpected()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "Test Book" };
            var user = new User { Id = "user123", UserName = "TestUser" };

            var notes = new Notes
            {
                Book = book,
                User = user
            };

            // Act & Assert
            Assert.That(notes.Book, Is.SameAs(book));
            Assert.That(notes.User, Is.SameAs(user));
        }

        [Test]
        [TestCase("user123", 1, " ", "Title", 1)]
        [TestCase("user123", 1, "", "Title", 1)]
        [TestCase("user123", 1, "Note content", " ", 1)]
        [TestCase("user123", 1, "Note content", "", 1)]
        public void RequiredFields_MissingValues_ValidationFails(
            string userId, int bookChapter, string noteContent, string title, int bookId)
        {
            // Arrange
            var notes = new Notes
            {
                UserId = userId,
                BookChapter = bookChapter,
                NoteContent = noteContent,
                Title = title,
                BookId = bookId
            };

            // Act
            var validationResults = ValidateModel(notes);

            // Assert
            Assert.That(validationResults.Count, Is.GreaterThan(0));

            // Check for specific error messages
            if (string.IsNullOrEmpty(noteContent))
            {
                Assert.That(validationResults.Any(vr => vr.ErrorMessage == "Полето е задължително"), Is.True);
            }

            if (string.IsNullOrEmpty(title))
            {
                Assert.That(validationResults.Any(vr => vr.ErrorMessage == "Името е заядължително"), Is.True);
            }
        }

        [Test]
        [TestCase(0)]
        public void BookChapter_ZeroOrNegative_ShouldFail(int invalidChapter)
        {
            // Arrange
            var notes = new Notes
            {
                UserId = "user123",
                BookChapter = invalidChapter,
                NoteContent = "Test content",
                Title = "Test title",
                BookId = 1
            };

            // Act
            var validationResults = ValidateModel(notes);

            // Assert
            // This test will only pass if you add a Range validation attribute to BookChapter
            // For example: [Range(1, int.MaxValue, ErrorMessage = "Главата трябва да е положително число")]
            // If you haven't added such validation, this test will fail
            Assert.That(validationResults.Any(v => v.MemberNames.Contains("BookChapter")), Is.True,
                "BookChapter should have validation errors for zero or negative values");
        }

        [Test]
        public void ForeignKeysAndNavigation_ManuallySetBoth_ValuesMatch()
        {
            // Arrange
            var book = new Book { Id = 5 };
            var user = new User { Id = "user123" };

            var notes = new Notes
            {
                // Explicitly set both navigation properties and foreign keys
                Book = book,
                BookId = book.Id,
                User = user,
                UserId = user.Id
            };

            // Act & Assert
            Assert.That(notes.BookId, Is.EqualTo(book.Id));
            Assert.That(notes.UserId, Is.EqualTo(user.Id));
            Assert.That(notes.Book, Is.SameAs(book));
            Assert.That(notes.User, Is.SameAs(user));
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
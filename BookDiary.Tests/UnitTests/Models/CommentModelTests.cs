using NUnit.Framework;
using BookDiary.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BookDiary.Tests.UnitTests.Models
{
    [TestFixture]
    public class CommentTests
    {
        [Test]
        public void Comment_IdProperty_ShouldHaveKeyAttribute()
        {
            var propertyInfo = typeof(Comment).GetProperty("Id");

            var keyAttribute = propertyInfo.GetCustomAttributes(typeof(KeyAttribute), false).FirstOrDefault();

            Assert.IsNotNull(keyAttribute, "Id property should have KeyAttribute");
        }

        [Test]
        public void Comment_UserIdProperty_ShouldHaveForeignKeyAttribute()
        {
            var propertyInfo = typeof(Comment).GetProperty("UserId");

            var foreignKeyAttribute = propertyInfo.GetCustomAttributes(typeof(ForeignKeyAttribute), false).FirstOrDefault() as ForeignKeyAttribute;

            Assert.IsNotNull(foreignKeyAttribute, "UserId property should have ForeignKeyAttribute");
            Assert.AreEqual("User", foreignKeyAttribute.Name);
        }

        [Test]
        public void Comment_BookIdProperty_ShouldHaveForeignKeyAttribute()
        {
            var propertyInfo = typeof(Comment).GetProperty("BookId");

            var foreignKeyAttribute = propertyInfo.GetCustomAttributes(typeof(ForeignKeyAttribute), false).FirstOrDefault() as ForeignKeyAttribute;

            Assert.IsNotNull(foreignKeyAttribute, "BookId property should have ForeignKeyAttribute");
            Assert.AreEqual("Book", foreignKeyAttribute.Name);
        }

        [Test]
        public void Comment_ContentProperty_ShouldHaveRequiredAttribute()
        {
            var propertyInfo = typeof(Comment).GetProperty("Content");

            var requiredAttribute = propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() as RequiredAttribute;

            Assert.IsNotNull(requiredAttribute, "Content property should have RequiredAttribute");
            Assert.AreEqual("Полето е задължително", requiredAttribute.ErrorMessage);
        }

        [Test]
        public void Comment_RatingProperty_ShouldHaveRequiredAttribute()
        {
            var propertyInfo = typeof(Comment).GetProperty("Rating");

            var requiredAttribute = propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() as RequiredAttribute;

            Assert.IsNotNull(requiredAttribute, "Rating property should have RequiredAttribute");
            Assert.AreEqual("Оценката е задължителна", requiredAttribute.ErrorMessage);
        }

        [TestCase(1, "user1", "Great book!", 5, 1)]
        [TestCase(2, "user2", "Enjoyed the story.", 4, 1)]
        [TestCase(3, "user1", "Average content.", 3, 2)]
        [TestCase(4, "user3", "Not my favorite.", 2, 3)]
        [TestCase(5, "user2", "Would not recommend.", 1, 3)]
        public void Comment_PropertiesSetCorrectly_ReturnsExpectedValues(int id, string userId, string content, int rating, int bookId)
        {
            var user = new User { Id = userId, UserName = "testuser" };
            var book = new Book { Id = bookId, Title = "Test Book" };

            var comment = new Comment
            {
                Id = id,
                UserId = userId,
                User = user,
                Content = content,
                Rating = rating,
                BookId = bookId,
                Book = book
            };

            Assert.AreEqual(id, comment.Id);
            Assert.AreEqual(userId, comment.UserId);
            Assert.AreEqual(user, comment.User);
            Assert.AreEqual(content, comment.Content);
            Assert.AreEqual(rating, comment.Rating);
            Assert.AreEqual(bookId, comment.BookId);
            Assert.AreEqual(book, comment.Book);
        }

        [Test]
        public void Comment_Validation_MissingContent_ShouldFailValidation()
        {
            var comment = new Comment
            {
                Id = 1,
                UserId = "user1",
                Rating = 5,
                BookId = 1
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(comment);

            var isValid = Validator.TryValidateObject(comment, validationContext, validationResults, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(validationResults.Any(vr => vr.MemberNames.Contains("Content")), "Should have a validation error for Content");
            Assert.IsTrue(validationResults.Any(vr => vr.ErrorMessage == "Полето е задължително"), "Should have the correct error message");
        }

        [Test]
        public void Comment_Validation_MissingUserId_ShouldFailValidation()
        {
            var comment = new Comment
            {
                Id = 1,
                Content = "Great book!",
                Rating = 5,
                BookId = 1
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(comment);

            var isValid = Validator.TryValidateObject(comment, validationContext, validationResults, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(validationResults.Any(vr => vr.MemberNames.Contains("UserId")),
                "Should have a validation error for UserId if it's configured as required");
        }

        [Test]
        public void Comment_Validation_ValidWithAllRequiredProperties()
        {
            var comment = new Comment
            {
                Id = 1,
                UserId = "user1",
                Content = "Great book!",
                Rating = 5,
                BookId = 1
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(comment);

            var isValid = Validator.TryValidateObject(comment, validationContext, validationResults, true);

            Assert.IsTrue(isValid);
            Assert.IsEmpty(validationResults);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase("   ")]
        public void Comment_EmptyOrWhitespaceContent_ShouldFailValidation(string invalidContent)
        {
            var comment = new Comment
            {
                Id = 1,
                UserId = "user1",
                Content = invalidContent,
                Rating = 5,
                BookId = 1
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(comment);

            var isValid = Validator.TryValidateObject(comment, validationContext, validationResults, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(validationResults.Any(vr => vr.MemberNames.Contains("Content")));
            Assert.IsTrue(validationResults.Any(vr => vr.ErrorMessage == "Полето е задължително"));
        }

        [TestCase(0)]
        [TestCase(6)]
        [TestCase(-1)]
        public void Comment_InvalidRatingValues_ShouldBeValidatedWithRangeAttribute(int invalidRating)
        {
            var comment = new Comment
            {
                Id = 1,
                UserId = "user1",
                Content = "Test comment",
                Rating = invalidRating,
                BookId = 1
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(comment);

            var isValid = Validator.TryValidateObject(comment, validationContext, validationResults, true);

            
            Assert.That(isValid, Is.False, "Rating should be validated with Range attribute");
            Assert.That(
                validationResults.Any(vr => vr.MemberNames.Contains("Rating")),
                Is.True,
                "Should have a validation error for Rating"
            );
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void Comment_ValidRatingValues_ShouldPassValidation(int validRating)
        {
            var comment = new Comment
            {
                Id = 1,
                UserId = "user1",
                Content = "Test comment",
                Rating = validRating,
                BookId = 1
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(comment);

            var isValid = Validator.TryValidateObject(comment, validationContext, validationResults, true);

            Assert.IsTrue(isValid, $"Rating of {validRating} should be valid");
            Assert.IsFalse(validationResults.Any(vr => vr.MemberNames.Contains("Rating")),
                "There should be no validation errors for Rating");
        }
    }
}
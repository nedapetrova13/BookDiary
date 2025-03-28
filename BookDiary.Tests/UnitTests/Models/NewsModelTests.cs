using NUnit.Framework;
using BookDiary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BookDiary.Tests.UnitTests.Models
{
    [TestFixture]
    public class NewsTests
    {
        [Test]
        public void News_IdProperty_ShouldHaveKeyAttribute()
        {
            // Arrange
            var propertyInfo = typeof(News).GetProperty("Id");

            // Act
            var keyAttribute = propertyInfo.GetCustomAttributes(typeof(KeyAttribute), false).FirstOrDefault();

            // Assert
            Assert.IsNotNull(keyAttribute, "Id property should have KeyAttribute");
        }

        [Test]
        public void News_TitleProperty_ShouldHaveRequiredAttribute()
        {
            // Arrange
            var propertyInfo = typeof(News).GetProperty("Title");

            // Act
            var requiredAttribute = propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() as RequiredAttribute;

            // Assert
            Assert.IsNotNull(requiredAttribute, "Title property should have RequiredAttribute");
            Assert.AreEqual("Името е заядължително", requiredAttribute.ErrorMessage);
        }

        [Test]
        public void News_ContentProperty_ShouldHaveRequiredAttribute()
        {
            // Arrange
            var propertyInfo = typeof(News).GetProperty("Content");

            // Act
            var requiredAttribute = propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() as RequiredAttribute;

            // Assert
            Assert.IsNotNull(requiredAttribute, "Content property should have RequiredAttribute");
            Assert.AreEqual("Полето е задължително", requiredAttribute.ErrorMessage);
        }

        [Test]
        public void News_CreatedProperty_ShouldHaveDefaultValue()
        {
            // Arrange
            var news = new News();

            // Act & Assert
            Assert.That(news.Created, Is.EqualTo(DateTime.Now).Within(TimeSpan.FromSeconds(1)));
        }

        [TestCase(1, "New Book Release", "Content about new book release")]
        [TestCase(2, "Author Interview", "Interview with a popular author")]
        [TestCase(3, "Book Fair Announcement", "Announcement about upcoming book fair")]
        [TestCase(4, "Award Winners", "List of recent literary award winners")]
        public void News_PropertiesSetCorrectly_ReturnsExpectedValues(int id, string title, string content)
        {
            // Arrange
            var created = DateTime.Now;
            var news = new News
            {
                Id = id,
                Title = title,
                Content = content,
                Created = created
            };

            // Act & Assert
            Assert.AreEqual(id, news.Id);
            Assert.AreEqual(title, news.Title);
            Assert.AreEqual(content, news.Content);
            Assert.AreEqual(created, news.Created);
        }

        [Test]
        public void News_Validation_InvalidWithMissingRequiredProperties()
        {
            // Arrange
            var news = new News
            {
                Id = 1
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(news);

            // Act
            var isValid = Validator.TryValidateObject(news, validationContext, validationResults, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.AreEqual(2, validationResults.Count);
            Assert.IsTrue(validationResults.Any(vr => vr.ErrorMessage == "Името е заядължително"));
            Assert.IsTrue(validationResults.Any(vr => vr.ErrorMessage == "Полето е задължително"));
        }

        [Test]
        public void News_Validation_InvalidWithMissingTitle()
        {
            // Arrange
            var news = new News
            {
                Id = 1,
                Content = "Test Content"
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(news);

            // Act
            var isValid = Validator.TryValidateObject(news, validationContext, validationResults, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.AreEqual(1, validationResults.Count);
            Assert.IsTrue(validationResults.Any(vr => vr.ErrorMessage == "Името е заядължително"));
        }

        [Test]
        public void News_Validation_InvalidWithMissingContent()
        {
            // Arrange
            var news = new News
            {
                Id = 1,
                Title = "Test Title"
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(news);

            // Act
            var isValid = Validator.TryValidateObject(news, validationContext, validationResults, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.AreEqual(1, validationResults.Count);
            Assert.IsTrue(validationResults.Any(vr => vr.ErrorMessage == "Полето е задължително"));
        }

        [Test]
        public void News_Validation_ValidWithAllRequiredProperties()
        {
            // Arrange
            var news = new News
            {
                Id = 1,
                Title = "Test Title",
                Content = "Test Content"
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(news);

            // Act
            var isValid = Validator.TryValidateObject(news, validationContext, validationResults, true);

            // Assert
            Assert.IsTrue(isValid);
            Assert.IsEmpty(validationResults);
        }

        [Test]
        public void News_CreatedDateTimeIsAutomaticallySet_WhenNotExplicitlyProvided()
        {
            // Arrange
            var beforeCreation = DateTime.Now.AddSeconds(-1);

            // Act
            var news = new News
            {
                Id = 1,
                Title = "Test Title",
                Content = "Test Content"
                // Not setting Created explicitly
            };
            var afterCreation = DateTime.Now.AddSeconds(1);

            // Assert
            Assert.That(news.Created, Is.GreaterThanOrEqualTo(beforeCreation));
            Assert.That(news.Created, Is.LessThanOrEqualTo(afterCreation));
        }

        [Test]
        public void News_CreatedDateTime_CanBeOverridden()
        {
            // Arrange
            var specificDate = new DateTime(2023, 1, 1, 12, 0, 0);

            // Act
            var news = new News
            {
                Id = 1,
                Title = "Test Title",
                Content = "Test Content",
                Created = specificDate
            };

            // Assert
            Assert.AreEqual(specificDate, news.Created);
        }

        [TestCase("")]
        [TestCase("   ")]
        public void News_EmptyOrNullTitle_ShouldFailValidation(string invalidTitle)
        {
            // Arrange
            var news = new News
            {
                Id = 1,
                Title = invalidTitle,
                Content = "Test Content"
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(news);

            // Act
            var isValid = Validator.TryValidateObject(news, validationContext, validationResults, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(validationResults.Any(vr => vr.ErrorMessage == "Името е заядължително"));
        }

        [TestCase("")]
        [TestCase("   ")]
        public void News_EmptyOrNullContent_ShouldFailValidation(string invalidContent)
        {
            // Arrange
            var news = new News
            {
                Id = 1,
                Title = "Test Title",
                Content = invalidContent
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(news);

            // Act
            var isValid = Validator.TryValidateObject(news, validationContext, validationResults, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(validationResults.Any(vr => vr.ErrorMessage == "Полето е задължително"));
        }
    }
}
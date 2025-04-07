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
            var propertyInfo = typeof(News).GetProperty("Id");

            var keyAttribute = propertyInfo.GetCustomAttributes(typeof(KeyAttribute), false).FirstOrDefault();

            Assert.IsNotNull(keyAttribute, "Id property should have KeyAttribute");
        }

        [Test]
        public void News_TitleProperty_ShouldHaveRequiredAttribute()
        {
            
            var propertyInfo = typeof(News).GetProperty("Title");

            var requiredAttribute = propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() as RequiredAttribute;

            Assert.IsNotNull(requiredAttribute, "Title property should have RequiredAttribute");
            Assert.AreEqual("Името е заядължително", requiredAttribute.ErrorMessage);
        }

        [Test]
        public void News_ContentProperty_ShouldHaveRequiredAttribute()
        {
            var propertyInfo = typeof(News).GetProperty("Content");

            var requiredAttribute = propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() as RequiredAttribute;

            Assert.IsNotNull(requiredAttribute, "Content property should have RequiredAttribute");
            Assert.AreEqual("Полето е задължително", requiredAttribute.ErrorMessage);
        }

        [Test]
        public void News_CreatedProperty_ShouldHaveDefaultValue()
        {
            var news = new News();

            Assert.That(news.Created, Is.EqualTo(DateTime.Now).Within(TimeSpan.FromSeconds(1)));
        }

        [TestCase(1, "New Book Release", "Content about new book release")]
        [TestCase(2, "Author Interview", "Interview with a popular author")]
        [TestCase(3, "Book Fair Announcement", "Announcement about upcoming book fair")]
        [TestCase(4, "Award Winners", "List of recent literary award winners")]
        public void News_PropertiesSetCorrectly_ReturnsExpectedValues(int id, string title, string content)
        {
            var created = DateTime.Now;
            var news = new News
            {
                Id = id,
                Title = title,
                Content = content,
                Created = created
            };

            Assert.AreEqual(id, news.Id);
            Assert.AreEqual(title, news.Title);
            Assert.AreEqual(content, news.Content);
            Assert.AreEqual(created, news.Created);
        }

        [Test]
        public void News_Validation_InvalidWithMissingRequiredProperties()
        {
            var news = new News
            {
                Id = 1
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(news);

            var isValid = Validator.TryValidateObject(news, validationContext, validationResults, true);

            Assert.IsFalse(isValid);
            Assert.AreEqual(2, validationResults.Count);
            Assert.IsTrue(validationResults.Any(vr => vr.ErrorMessage == "Името е заядължително"));
            Assert.IsTrue(validationResults.Any(vr => vr.ErrorMessage == "Полето е задължително"));
        }

        [Test]
        public void News_Validation_InvalidWithMissingTitle()
        {
            var news = new News
            {
                Id = 1,
                Content = "Test Content"
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(news);

            var isValid = Validator.TryValidateObject(news, validationContext, validationResults, true);

            Assert.IsFalse(isValid);
            Assert.AreEqual(1, validationResults.Count);
            Assert.IsTrue(validationResults.Any(vr => vr.ErrorMessage == "Името е заядължително"));
        }

        [Test]
        public void News_Validation_InvalidWithMissingContent()
        {
            var news = new News
            {
                Id = 1,
                Title = "Test Title"
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(news);

            var isValid = Validator.TryValidateObject(news, validationContext, validationResults, true);

            Assert.IsFalse(isValid);
            Assert.AreEqual(1, validationResults.Count);
            Assert.IsTrue(validationResults.Any(vr => vr.ErrorMessage == "Полето е задължително"));
        }

        [Test]
        public void News_Validation_ValidWithAllRequiredProperties()
        {
            var news = new News
            {
                Id = 1,
                Title = "Test Title",
                Content = "Test Content"
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(news);

            var isValid = Validator.TryValidateObject(news, validationContext, validationResults, true);

            Assert.IsTrue(isValid);
            Assert.IsEmpty(validationResults);
        }

        [Test]
        public void News_CreatedDateTimeIsAutomaticallySet_WhenNotExplicitlyProvided()
        {
            var beforeCreation = DateTime.Now.AddSeconds(-1);

            var news = new News
            {
                Id = 1,
                Title = "Test Title",
                Content = "Test Content"
            };
            var afterCreation = DateTime.Now.AddSeconds(1);

            Assert.That(news.Created, Is.GreaterThanOrEqualTo(beforeCreation));
            Assert.That(news.Created, Is.LessThanOrEqualTo(afterCreation));
        }

        [Test]
        public void News_CreatedDateTime_CanBeOverridden()
        {
            var specificDate = new DateTime(2023, 1, 1, 12, 0, 0);

            var news = new News
            {
                Id = 1,
                Title = "Test Title",
                Content = "Test Content",
                Created = specificDate
            };

            Assert.AreEqual(specificDate, news.Created);
        }

        [TestCase("")]
        [TestCase("   ")]
        public void News_EmptyOrNullTitle_ShouldFailValidation(string invalidTitle)
        {
            var news = new News
            {
                Id = 1,
                Title = invalidTitle,
                Content = "Test Content"
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(news);

            var isValid = Validator.TryValidateObject(news, validationContext, validationResults, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(validationResults.Any(vr => vr.ErrorMessage == "Името е заядължително"));
        }

        [TestCase("")]
        [TestCase("   ")]
        public void News_EmptyOrNullContent_ShouldFailValidation(string invalidContent)
        {
            var news = new News
            {
                Id = 1,
                Title = "Test Title",
                Content = invalidContent
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(news);

            var isValid = Validator.TryValidateObject(news, validationContext, validationResults, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(validationResults.Any(vr => vr.ErrorMessage == "Полето е задължително"));
        }
    }
}
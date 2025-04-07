using NUnit.Framework;
using BookDiary.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BookDiary.Tests.UnitTests.Models
{
    [TestFixture]
    public class TagTests
    {
        [Test]
        public void Tag_IdProperty_ShouldHaveKeyAttribute()
        {
            var propertyInfo = typeof(Tag).GetProperty("Id");

            var keyAttribute = propertyInfo.GetCustomAttributes(typeof(KeyAttribute), false).FirstOrDefault();

            Assert.IsNotNull(keyAttribute, "Id property should have KeyAttribute");
        }

        [Test]
        public void Tag_NameProperty_ShouldHaveRequiredAttribute()
        {
            var propertyInfo = typeof(Tag).GetProperty("Name");

            var requiredAttribute = propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() as RequiredAttribute;

            Assert.IsNotNull(requiredAttribute, "Name property should have RequiredAttribute");
            Assert.AreEqual("Името е заядължително", requiredAttribute.ErrorMessage);
        }

        [TestCase(1, "Fantasy")]
        [TestCase(2, "Science Fiction")]
        [TestCase(3, "Mystery")]
        [TestCase(4, "Romance")]
        [TestCase(5, "History")]
        public void Tag_PropertiesSetCorrectly_ReturnsExpectedValues(int id, string name)
        {
            var tag = new Tag
            {
                Id = id,
                Name = name,
                BookTags = new List<BookTag>()
            };

            Assert.AreEqual(id, tag.Id);
            Assert.AreEqual(name, tag.Name);
            Assert.IsNotNull(tag.BookTags);
            Assert.IsEmpty(tag.BookTags);
        }

        [Test]
        public void Tag_BookTagsCollection_CanAddAndRemoveBookTags()
        {
            var tag = new Tag
            {
                Id = 1,
                Name = "Fantasy",
                BookTags = new List<BookTag>()
            };

            var bookTag1 = new BookTag { TagId = 1, BookId = 1 };
            var bookTag2 = new BookTag { TagId = 1, BookId = 2 };

            tag.BookTags.Add(bookTag1);
            tag.BookTags.Add(bookTag2);

            Assert.AreEqual(2, tag.BookTags.Count);
            Assert.IsTrue(tag.BookTags.Contains(bookTag1));
            Assert.IsTrue(tag.BookTags.Contains(bookTag2));

            tag.BookTags.Remove(bookTag1);

            Assert.AreEqual(1, tag.BookTags.Count);
            Assert.IsFalse(tag.BookTags.Contains(bookTag1));
            Assert.IsTrue(tag.BookTags.Contains(bookTag2));
        }

        [Test]
        public void Tag_Validation_InvalidWithMissingRequiredProperties()
        {
            var tag = new Tag
            {
                Id = 1
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(tag);

            var isValid = Validator.TryValidateObject(tag, validationContext, validationResults, true);

            Assert.IsFalse(isValid);
            Assert.AreEqual(1, validationResults.Count);
            Assert.IsTrue(validationResults.Any(vr => vr.ErrorMessage == "Името е заядължително"));
        }

        [Test]
        public void Tag_Validation_ValidWithAllRequiredProperties()
        {
            var tag = new Tag
            {
                Id = 1,
                Name = "Fantasy",
                BookTags = new List<BookTag>()
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(tag);

            var isValid = Validator.TryValidateObject(tag, validationContext, validationResults, true);

            Assert.IsTrue(isValid);
            Assert.IsEmpty(validationResults);
        }

        [Test]
        public void Tag_WithMultipleBookTags_ShouldWorkCorrectly()
        {
            var tag = new Tag
            {
                Id = 1,
                Name = "Fantasy",
                BookTags = new List<BookTag>()
            };

            for (int i = 1; i <= 5; i++)
            {
                tag.BookTags.Add(new BookTag { TagId = 1, BookId = i });
            }

            Assert.AreEqual(5, tag.BookTags.Count);

            var bookIds = tag.BookTags.Select(bt => bt.BookId).ToList();
            for (int i = 1; i <= 5; i++)
            {
                Assert.IsTrue(bookIds.Contains(i), $"Book ID {i} should be in the collection");
            }
        }
    }
}
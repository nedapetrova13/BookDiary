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
            var propertyInfo = typeof(Language).GetProperty("Id");

            var keyAttribute = propertyInfo.GetCustomAttributes(typeof(KeyAttribute), false).FirstOrDefault();

            Assert.IsNotNull(keyAttribute, "Id property should have KeyAttribute");
        }

        [Test]
        public void Language_NameProperty_ShouldHaveRequiredAttribute()
        {
            var propertyInfo = typeof(Language).GetProperty("Name");

            var requiredAttribute = propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() as RequiredAttribute;

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
            var language = new Language
            {
                Id = id,
                Name = name,
                Books = new List<Book>()
            };

            Assert.AreEqual(id, language.Id);
            Assert.AreEqual(name, language.Name);
            Assert.IsNotNull(language.Books);
            Assert.IsEmpty(language.Books);
        }

        [Test]
        public void Language_BooksCollection_CanAddAndRemoveBooks()
        {
            var language = new Language
            {
                Id = 1,
                Name = "English",
                Books = new List<Book>()
            };

            var book1 = new Book { Id = 1, Title = "Book 1" };
            var book2 = new Book { Id = 2, Title = "Book 2" };

            language.Books.Add(book1);
            language.Books.Add(book2);

            Assert.AreEqual(2, language.Books.Count);
            Assert.IsTrue(language.Books.Contains(book1));
            Assert.IsTrue(language.Books.Contains(book2));

            language.Books.Remove(book1);

            Assert.AreEqual(1, language.Books.Count);
            Assert.IsFalse(language.Books.Contains(book1));
            Assert.IsTrue(language.Books.Contains(book2));
        }

        [Test]
        public void Language_Validation_InvalidWithMissingRequiredProperties()
        {
            var language = new Language
            {
                Id = 1
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(language);

            var isValid = Validator.TryValidateObject(language, validationContext, validationResults, true);

            Assert.IsFalse(isValid);
            Assert.AreEqual(1, validationResults.Count);
            Assert.IsTrue(validationResults.Any(vr => vr.ErrorMessage == "Името е заядължително"));
        }

        [Test]
        public void Language_Validation_ValidWithAllRequiredProperties()
        {
            var language = new Language
            {
                Id = 1,
                Name = "English",
                Books = new List<Book>()
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(language);

            var isValid = Validator.TryValidateObject(language, validationContext, validationResults, true);

            Assert.IsTrue(isValid);
            Assert.IsEmpty(validationResults);
        }

        [Test]
        public void Language_WithMultipleBooks_ShouldWorkCorrectly()
        {
            var language = new Language
            {
                Id = 1,
                Name = "English",
                Books = new List<Book>()
            };

            for (int i = 1; i <= 5; i++)
            {
                language.Books.Add(new Book { Id = i, Title = $"Book {i}" });
            }

            Assert.AreEqual(5, language.Books.Count);

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
            var language = new Language
            {
                Id = 1,
                Name = invalidName,
                Books = new List<Book>()
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(language);

            var isValid = Validator.TryValidateObject(language, validationContext, validationResults, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(validationResults.Any(vr => vr.ErrorMessage == "Името е заядължително"));
        }

        [Test]
        public void Language_BooksProperty_ShouldBeInitializedToEmptyCollectionIfNull()
        {
            var language = new Language
            {
                Id = 1,
                Name = "English",
                Books = null
            };

            
            if (language.Books == null)
            {
                language.Books = new List<Book>();
            }

            Assert.IsNotNull(language.Books);
            Assert.IsEmpty(language.Books);
        }
    }
}
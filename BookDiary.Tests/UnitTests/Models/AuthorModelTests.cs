using NUnit.Framework;
using BookDiary.Models;
using BookDiary.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BookDiary.Tests.UnitTests.Models
{
    [TestFixture]
    public class AuthorTests
    {
        [Test]
        public void Author_IdProperty_ShouldHaveKeyAttribute()
        {
            // Arrange
            var propertyInfo = typeof(Author).GetProperty("Id");

            // Act
            var keyAttribute = propertyInfo.GetCustomAttributes(typeof(KeyAttribute), false).FirstOrDefault();

            // Assert
            Assert.IsNotNull(keyAttribute, "Id property should have KeyAttribute");
        }

        [Test]
        public void Author_NameProperty_ShouldHaveRequiredAttribute()
        {
            // Arrange
            var propertyInfo = typeof(Author).GetProperty("Name");

            // Act
            var requiredAttribute = propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() as RequiredAttribute;

            // Assert
            Assert.IsNotNull(requiredAttribute, "Name property should have RequiredAttribute");
            Assert.AreEqual("Името е заядължително", requiredAttribute.ErrorMessage);
        }

        [Test]
        public void Author_EmailProperty_ShouldHaveRequiredAttribute()
        {
            // Arrange
            var propertyInfo = typeof(Author).GetProperty("Email");

            // Act
            var requiredAttribute = propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() as RequiredAttribute;

            // Assert
            Assert.IsNotNull(requiredAttribute, "Email property should have RequiredAttribute");
            Assert.AreEqual("Email е заядължителен", requiredAttribute.ErrorMessage);
        }

        [Test]
        public void Author_ProfilePictureURLProperty_ShouldHaveRequiredAttribute()
        {
            // Arrange
            var propertyInfo = typeof(Author).GetProperty("ProfilePictureURL");

            // Act
            var requiredAttribute = propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() as RequiredAttribute;

            // Assert
            Assert.IsNotNull(requiredAttribute, "ProfilePictureURL property should have RequiredAttribute");
            Assert.AreEqual("Полето е задължително", requiredAttribute.ErrorMessage);
        }

        [Test]
        public void Author_BioProperty_ShouldHaveRequiredAttribute()
        {
            // Arrange
            var propertyInfo = typeof(Author).GetProperty("Bio");

            // Act
            var requiredAttribute = propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() as RequiredAttribute;

            // Assert
            Assert.IsNotNull(requiredAttribute, "Bio property should have RequiredAttribute");
            Assert.AreEqual("Полето е задължително", requiredAttribute.ErrorMessage);
        }

        [Test]
        public void Author_WebSiteLinkProperty_ShouldHaveRequiredAttribute()
        {
            // Arrange
            var propertyInfo = typeof(Author).GetProperty("WebSiteLink");

            // Act
            var requiredAttribute = propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() as RequiredAttribute;

            // Assert
            Assert.IsNotNull(requiredAttribute, "WebSiteLink property should have RequiredAttribute");
            Assert.AreEqual("Полето е задължително", requiredAttribute.ErrorMessage);
        }

        [Test]
        public void Author_GenderProperty_ShouldHaveEnumDataTypeAttribute()
        {
            // Arrange
            var propertyInfo = typeof(Author).GetProperty("Gender");

            // Act
            var enumDataTypeAttribute = propertyInfo.GetCustomAttributes(typeof(EnumDataTypeAttribute), false).FirstOrDefault() as EnumDataTypeAttribute;

            // Assert
            Assert.IsNotNull(enumDataTypeAttribute, "Gender property should have EnumDataTypeAttribute");
            Assert.AreEqual(typeof(GenderEnum), enumDataTypeAttribute.EnumType);
        }

        [TestCase(1, "J.K. Rowling", "jkrowling@example.com", "https://example.com/jkrowling.jpg", "Жена", "British author best known for Harry Potter.", "https://jkrowling.com")]
        [TestCase(2, "Stephen King", "sking@example.com", "https://example.com/sking.jpg", "Мъж", "American author of horror, supernatural fiction, suspense, and fantasy novels.", "https://stephenking.com")]
        [TestCase(3, "Agatha Christie", "agatha@example.com", "https://example.com/agatha.jpg", "Жена", "English writer known for her detective novels.", "https://agathachristie.com")]
        public void Author_PropertiesSetCorrectly_ReturnsExpectedValues(int id, string name, string email, string profilePictureURL, string gender, string bio, string webSiteLink)
        {
            // Arrange
            var birthDate = new DateTime(1965, 7, 31);
            var author = new Author
            {
                Id = id,
                Name = name,
                Email = email,
                ProfilePictureURL = profilePictureURL,
                Gender = gender,
                Bio = bio,
                WebSiteLink = webSiteLink,
                BirthDate = birthDate,
                Books = new List<Book>(),
                Series = new List<Series>()
            };

            // Act & Assert
            Assert.AreEqual(id, author.Id);
            Assert.AreEqual(name, author.Name);
            Assert.AreEqual(email, author.Email);
            Assert.AreEqual(profilePictureURL, author.ProfilePictureURL);
            Assert.AreEqual(gender, author.Gender);
            Assert.AreEqual(bio, author.Bio);
            Assert.AreEqual(webSiteLink, author.WebSiteLink);
            Assert.AreEqual(birthDate, author.BirthDate);
            Assert.IsNotNull(author.Books);
            Assert.IsEmpty(author.Books);
            Assert.IsNotNull(author.Series);
            Assert.IsEmpty(author.Series);
        }

        [Test]
        public void Author_BooksCollection_CanAddAndRemoveBooks()
        {
            // Arrange
            var author = new Author
            {
                Id = 1,
                Name = "Test Author",
                Email = "test@example.com",
                ProfilePictureURL = "https://example.com/test.jpg",
                Bio = "Test bio",
                WebSiteLink = "https://example.com",
                Books = new List<Book>()
            };

            var book1 = new Book { Id = 1, Title = "Book 1" };
            var book2 = new Book { Id = 2, Title = "Book 2" };

            // Act
            author.Books.Add(book1);
            author.Books.Add(book2);

            // Assert
            Assert.AreEqual(2, author.Books.Count);
            Assert.IsTrue(author.Books.Contains(book1));
            Assert.IsTrue(author.Books.Contains(book2));

            // Act
            author.Books.Remove(book1);

            // Assert
            Assert.AreEqual(1, author.Books.Count);
            Assert.IsFalse(author.Books.Contains(book1));
            Assert.IsTrue(author.Books.Contains(book2));
        }

        [Test]
        public void Author_SeriesCollection_CanAddAndRemoveSeries()
        {
            // Arrange
            var author = new Author
            {
                Id = 1,
                Name = "Test Author",
                Email = "test@example.com",
                ProfilePictureURL = "https://example.com/test.jpg",
                Bio = "Test bio",
                WebSiteLink = "https://example.com",
                Series = new List<Series>()
            };

            var series1 = new Series { Id = 1, Title = "Series 1", Description = "Description 1" };
            var series2 = new Series { Id = 2, Title = "Series 2", Description = "Description 2" };

            // Act
            author.Series.Add(series1);
            author.Series.Add(series2);

            // Assert
            Assert.AreEqual(2, author.Series.Count);
            Assert.IsTrue(author.Series.Contains(series1));
            Assert.IsTrue(author.Series.Contains(series2));

            // Act
            author.Series.Remove(series1);

            // Assert
            Assert.AreEqual(1, author.Series.Count);
            Assert.IsFalse(author.Series.Contains(series1));
            Assert.IsTrue(author.Series.Contains(series2));
        }

        [Test]
        public void Author_Validation_InvalidWithMissingRequiredProperties()
        {
            // Arrange
            var author = new Author
            {
                Id = 1,
                BirthDate = new DateTime(1965, 7, 31)
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(author);

            // Act
            var isValid = Validator.TryValidateObject(author, validationContext, validationResults, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.AreEqual(5, validationResults.Count); // Name, Email, ProfilePictureURL, Bio, WebSiteLink
            Assert.IsTrue(validationResults.Any(vr => vr.ErrorMessage == "Името е заядължително"));
            Assert.IsTrue(validationResults.Any(vr => vr.ErrorMessage == "Email е заядължителен"));
            Assert.IsTrue(validationResults.Any(vr => vr.ErrorMessage == "Полето е задължително"));
        }

        [Test]
        public void Author_Validation_ValidWithAllRequiredProperties()
        {
            // Arrange
            var author = new Author
            {
                Id = 1,
                Name = "Test Author",
                Email = "test@example.com",
                ProfilePictureURL = "https://example.com/test.jpg",
                Bio = "Test bio",
                WebSiteLink = "https://example.com",
                BirthDate = new DateTime(1965, 7, 31),
                Books = new List<Book>(),
                Series = new List<Series>()
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(author);

            // Act
            var isValid = Validator.TryValidateObject(author, validationContext, validationResults, true);

            // Assert
            Assert.IsTrue(isValid);
            Assert.IsEmpty(validationResults);
        }

        [TestCase("")]
        [TestCase("   ")]
        public void Author_EmptyOrNullName_ShouldFailValidation(string invalidName)
        {
            // Arrange
            var author = new Author
            {
                Id = 1,
                Name = invalidName,
                Email = "test@example.com",
                ProfilePictureURL = "https://example.com/test.jpg",
                Bio = "Test bio",
                WebSiteLink = "https://example.com"
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(author);

            // Act
            var isValid = Validator.TryValidateObject(author, validationContext, validationResults, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(validationResults.Any(vr => vr.ErrorMessage == "Името е заядължително"));
        }

        [TestCase("Мъж")]
        [TestCase("Жена")]
        public void Author_GenderValues_ShouldBeValidatedAgainstEnum(string gender)
        {
            // Arrange
            var author = new Author
            {
                Id = 1,
                Name = "Test Author",
                Email = "test@example.com",
                ProfilePictureURL = "https://example.com/test.jpg",
                Bio = "Test bio",
                WebSiteLink = "https://example.com",
                Gender = gender
            };

            // Act & Assert
            if (gender == null)
            {
                // Null should be valid since Gender is nullable
                Assert.IsNull(author.Gender);
            }
            else
            {
                // Check if the gender value is valid according to GenderEnum
                Assert.That(IsValidGender(gender), Is.True);
            }
        }

        [Test]
        public void Author_NullableCollections_ShouldBeAllowed()
        {
            // Arrange
            var author = new Author
            {
                Id = 1,
                Name = "Test Author",
                Email = "test@example.com",
                ProfilePictureURL = "https://example.com/test.jpg",
                Bio = "Test bio",
                WebSiteLink = "https://example.com",
                Books = null,
                Series = null
            };

            // Act & Assert
            Assert.IsNull(author.Books);
            Assert.IsNull(author.Series);

            // This test verifies that the Books and Series properties can be null as indicated by the ? in the model
        }

        // Helper method to validate gender values against GenderEnum
        private bool IsValidGender(string gender)
        {
            return Enum.GetNames(typeof(GenderEnum)).Contains(gender);
        }
    }
}
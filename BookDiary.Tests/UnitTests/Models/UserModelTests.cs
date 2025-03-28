using NUnit.Framework;
using BookDiary.Models;
using BookDiary.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace BookDiary.Tests.UnitTests.Models
{
    [TestFixture]
    public class UserTests
    {
        [Test]
        public void Constructor_Default_PropertiesInitializedCorrectly()
        {
            // Arrange & Act
            var user = new User();

            // Assert
            Assert.That(user.Name, Is.Null);
            Assert.That(user.Birthdate, Is.EqualTo(default(DateTime)));
            Assert.That(user.Gender, Is.Null);
            Assert.That(user.Shelves, Is.Null);
            Assert.That(user.ProfilePictureURL, Is.Null);
            Assert.That(user.Bio, Is.Null);
            Assert.That(user.CurrentReads, Is.Null);
            Assert.That(user.Notes, Is.Null);
            Assert.That(user.MyComments, Is.Null);
        }

        [Test]
        [TestCase("John Doe", "2000-01-01", "Male", "profile.jpg", "Book lover")]
        [TestCase("Jane Smith", "1995-05-15", "Female", "jane.png", "Sci-fi enthusiast")]
        [TestCase("Alex Johnson", "1988-12-25", "NonBinary", " ", " ")]
        public void Properties_SetAndGet_ValuesMatchExpected(
            string name, string birthdate, string gender, string profilePic, string bio)
        {
            // Arrange
            var user = new User
            {
                Name = name,
                Birthdate = DateTime.Parse(birthdate),
                Gender = gender,
                ProfilePictureURL = profilePic,
                Bio = bio
            };

            // Act & Assert
            Assert.That(user.Name, Is.EqualTo(name));
            Assert.That(user.Birthdate.Date, Is.EqualTo(DateTime.Parse(birthdate).Date));
            Assert.That(user.Gender, Is.EqualTo(gender));
            Assert.That(user.ProfilePictureURL, Is.EqualTo(profilePic));
            Assert.That(user.Bio, Is.EqualTo(bio));
        }

        [Test]
        public void IdentityUserProperties_Inherited_AccessibleAndSettable()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var email = "test@example.com";
            var userName = "testuser";
            var phoneNumber = "1234567890";

            var user = new User
            {
                Id = userId,
                Email = email,
                UserName = userName,
                PhoneNumber = phoneNumber
            };

            // Act & Assert
            Assert.That(user.Id, Is.EqualTo(userId));
            Assert.That(user.Email, Is.EqualTo(email));
            Assert.That(user.UserName, Is.EqualTo(userName));
            Assert.That(user.PhoneNumber, Is.EqualTo(phoneNumber));
        }

        [Test]
        public void CollectionProperties_InitializeEmpty_WorksCorrectly()
        {
            // Arrange
            var user = new User
            {
                Shelves = new List<Shelf>(),
                CurrentReads = new List<CurrentRead>(),
                Notes = new List<Notes>(),
                MyComments = new List<Comment>()
            };

            // Act & Assert
            Assert.That(user.Shelves, Is.Not.Null);
            Assert.That(user.Shelves.Count, Is.EqualTo(0));
            Assert.That(user.CurrentReads, Is.Not.Null);
            Assert.That(user.CurrentReads.Count, Is.EqualTo(0));
            Assert.That(user.Notes, Is.Not.Null);
            Assert.That(user.Notes.Count, Is.EqualTo(0));
            Assert.That(user.MyComments, Is.Not.Null);
            Assert.That(user.MyComments.Count, Is.EqualTo(0));
        }

        [Test]
        public void CollectionProperties_AddItems_WorksCorrectly()
        {
            // Arrange
            var user = new User
            {
                Shelves = new List<Shelf>(),
                CurrentReads = new List<CurrentRead>(),
                Notes = new List<Notes>(),
                MyComments = new List<Comment>()
            };

            var shelf = new Shelf { Id = 1, Name = "Test Shelf" };
            var currentRead = new CurrentRead { Id = 1, CurrentPage = 100 };
            var note = new Notes { Id = 1, Title = "Test Note" };
            var comment = new Comment { Id = 1, Content = "Test Comment" };

            // Act
            user.Shelves.Add(shelf);
            user.CurrentReads.Add(currentRead);
            user.Notes.Add(note);
            user.MyComments.Add(comment);

            // Assert
            Assert.That(user.Shelves.Count, Is.EqualTo(1));
            Assert.That(user.Shelves.First(), Is.SameAs(shelf));

            Assert.That(user.CurrentReads.Count, Is.EqualTo(1));
            Assert.That(user.CurrentReads.First(), Is.SameAs(currentRead));

            Assert.That(user.Notes.Count, Is.EqualTo(1));
            Assert.That(user.Notes.First(), Is.SameAs(note));

            Assert.That(user.MyComments.Count, Is.EqualTo(1));
            Assert.That(user.MyComments.First(), Is.SameAs(comment));
        }

        [Test]
        [TestCase(" ")]
        [TestCase("")]
        [TestCase("   ")]
        public void Name_RequiredValidation_ValidationFails(string invalidName)
        {
            // Arrange
            var user = new User
            {
                Name = invalidName
            };

            // Act
            var validationResults = ValidateModel(user);

            // Assert
            Assert.That(validationResults.Any(v => v.MemberNames.Contains("Name")), Is.True);
            Assert.That(validationResults.Any(v => v.ErrorMessage == "Името е задължително"), Is.True);
        }

        [Test]
        [TestCase("Invalid")]
        [TestCase("NotAnEnum")]
        public void Gender_EnumValidation_ValidationFails(string invalidGender)
        {
            // Arrange
            var user = new User
            {
                Name = "Test User",
                Gender = invalidGender
            };

            // Act
            var validationResults = ValidateModel(user);

            // Assert
            Assert.That(validationResults.Any(v => v.MemberNames.Contains("Gender")), Is.True);
        }

        [Test]
        [TestCase("Мъж")]
        [TestCase("Жена")]
        public void Gender_ValidEnumValues_ValidationPasses(string validGender)
        {
            // Arrange
            var user = new User
            {
                Name = "Test User",
                Gender = validGender
            };

            // Act
            var validationResults = ValidateModel(user);

            // Assert
            Assert.That(validationResults.Any(v => v.MemberNames.Contains("Gender")), Is.False);
        }

        [Test]
        public void Constructor_WithParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var name = "Test User";
            var birthdate = new DateTime(1990, 1, 1);
            var gender = "Male";

            // Act
            var user = new User
            {
                Name = name,
                Birthdate = birthdate,
                Gender = gender
            };

            // Assert
            Assert.That(user.Name, Is.EqualTo(name));
            Assert.That(user.Birthdate, Is.EqualTo(birthdate));
            Assert.That(user.Gender, Is.EqualTo(gender));
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
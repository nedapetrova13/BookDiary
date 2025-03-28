using NUnit.Framework;
using BookDiary.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BookDiary.Tests.UnitTests.Models
{
    [TestFixture]
    public class ShelfTests
    {
        [Test]
        public void Shelf_IdProperty_ShouldHaveKeyAttribute()
        {
            // Arrange
            var propertyInfo = typeof(Shelf).GetProperty("Id");

            // Act
            var keyAttribute = propertyInfo.GetCustomAttributes(typeof(KeyAttribute), false).FirstOrDefault();

            // Assert
            Assert.IsNotNull(keyAttribute, "Id property should have KeyAttribute");
        }

        [Test]
        public void Shelf_NameProperty_ShouldHaveRequiredAttribute()
        {
            // Arrange
            var propertyInfo = typeof(Shelf).GetProperty("Name");

            // Act
            var requiredAttribute = propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() as RequiredAttribute;

            // Assert
            Assert.IsNotNull(requiredAttribute, "Name property should have RequiredAttribute");
            Assert.AreEqual("Името е заядължително", requiredAttribute.ErrorMessage);
        }

        [Test]
        public void Shelf_DescriptionProperty_ShouldHaveRequiredAttribute()
        {
            // Arrange
            var propertyInfo = typeof(Shelf).GetProperty("Description");

            // Act
            var requiredAttribute = propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() as RequiredAttribute;

            // Assert
            Assert.IsNotNull(requiredAttribute, "Description property should have RequiredAttribute");
            Assert.AreEqual("Полето е задължително", requiredAttribute.ErrorMessage);
        }

        [Test]
        public void Shelf_UserIdProperty_ShouldHaveForeignKeyAttribute()
        {
            // Arrange
            var propertyInfo = typeof(Shelf).GetProperty("UserId");

            // Act
            var foreignKeyAttribute = propertyInfo.GetCustomAttributes(typeof(ForeignKeyAttribute), false).FirstOrDefault() as ForeignKeyAttribute;

            // Assert
            Assert.IsNotNull(foreignKeyAttribute, "UserId property should have ForeignKeyAttribute");
            Assert.AreEqual("User", foreignKeyAttribute.Name);
        }

        [TestCase(1, "To Read", "Books I want to read", "user1")]
        [TestCase(2, "Currently Reading", "Books I'm reading now", "user1")]
        [TestCase(3, "Favorites", "My favorite books", "user2")]
        [TestCase(4, "Wishlist", "Books I want to buy", "user3")]
        public void Shelf_PropertiesSetCorrectly_ReturnsExpectedValues(int id, string name, string description, string userId)
        {
            // Arrange
            var user = new User { Id = userId, UserName = "testuser" };
            var shelf = new Shelf
            {
                Id = id,
                Name = name,
                Description = description,
                UserId = userId,
                User = user,
                ShelfBooks = new List<ShelfBook>()
            };

            // Act & Assert
            Assert.AreEqual(id, shelf.Id);
            Assert.AreEqual(name, shelf.Name);
            Assert.AreEqual(description, shelf.Description);
            Assert.AreEqual(userId, shelf.UserId);
            Assert.AreEqual(user, shelf.User);
            Assert.IsNotNull(shelf.ShelfBooks);
            Assert.IsEmpty(shelf.ShelfBooks);
        }

        [Test]
        public void Shelf_ShelfBooksCollection_CanAddAndRemoveShelfBooks()
        {
            // Arrange
            var shelf = new Shelf
            {
                Id = 1,
                Name = "Test Shelf",
                Description = "Test Description",
                UserId = "user1",
                User = new User { Id = "user1", UserName = "testuser" },
                ShelfBooks = new List<ShelfBook>()
            };

            var shelfBook1 = new ShelfBook { ShelfId = 1, BookId = 1 };
            var shelfBook2 = new ShelfBook { ShelfId = 1, BookId = 2 };

            // Act
            shelf.ShelfBooks.Add(shelfBook1);
            shelf.ShelfBooks.Add(shelfBook2);

            // Assert
            Assert.AreEqual(2, shelf.ShelfBooks.Count);
            Assert.IsTrue(shelf.ShelfBooks.Contains(shelfBook1));
            Assert.IsTrue(shelf.ShelfBooks.Contains(shelfBook2));

            // Act
            shelf.ShelfBooks.Remove(shelfBook1);

            // Assert
            Assert.AreEqual(1, shelf.ShelfBooks.Count);
            Assert.IsFalse(shelf.ShelfBooks.Contains(shelfBook1));
            Assert.IsTrue(shelf.ShelfBooks.Contains(shelfBook2));
        }

        [Test]
        public void Shelf_Validation_InvalidWithMissingRequiredProperties()
        {
            // Arrange
            var shelf = new Shelf
            {
                Id = 1,
                UserId = "user1",
                User = new User { Id = "user1", UserName = "testuser" }
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(shelf);

            // Act
            var isValid = Validator.TryValidateObject(shelf, validationContext, validationResults, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.AreEqual(2, validationResults.Count);
            Assert.IsTrue(validationResults.Any(vr => vr.ErrorMessage == "Името е заядължително"));
            Assert.IsTrue(validationResults.Any(vr => vr.ErrorMessage == "Полето е задължително"));
        }

        [Test]
        public void Shelf_Validation_ValidWithAllRequiredProperties()
        {
            // Arrange
            var shelf = new Shelf
            {
                Id = 1,
                Name = "Test Shelf",
                Description = "Test Description",
                UserId = "user1",
                User = new User { Id = "user1", UserName = "testuser" },
                ShelfBooks = new List<ShelfBook>()
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(shelf);

            // Act
            var isValid = Validator.TryValidateObject(shelf, validationContext, validationResults, true);

            // Assert
            Assert.IsTrue(isValid);
            Assert.IsEmpty(validationResults);
        }

        [Test]
        public void Shelf_WithMultipleShelfBooks_ShouldWorkCorrectly()
        {
            // Arrange
            var shelf = new Shelf
            {
                Id = 1,
                Name = "Reading List",
                Description = "My current reading list",
                UserId = "user1",
                User = new User { Id = "user1", UserName = "testuser" },
                ShelfBooks = new List<ShelfBook>()
            };

            // Add multiple shelf books
            for (int i = 1; i <= 5; i++)
            {
                shelf.ShelfBooks.Add(new ShelfBook { ShelfId = 1, BookId = i });
            }

            // Act & Assert
            Assert.AreEqual(5, shelf.ShelfBooks.Count);

            // Verify all expected BookIds are present
            var bookIds = shelf.ShelfBooks.Select(sb => sb.BookId).ToList();
            for (int i = 1; i <= 5; i++)
            {
                Assert.IsTrue(bookIds.Contains(i), $"Book ID {i} should be in the collection");
            }
        }

        [Test]
        public void Shelf_NullableShelfBooks_ShouldBeAllowed()
        {
            // Arrange
            var shelf = new Shelf
            {
                Id = 1,
                Name = "Empty Shelf",
                Description = "A shelf with no books",
                UserId = "user1",
                User = new User { Id = "user1", UserName = "testuser" },
                ShelfBooks = null
            };

            // Act & Assert
            Assert.IsNull(shelf.ShelfBooks);

            // This test verifies that the ShelfBooks property can be null as indicated by the ? in the model
        }
    }
}
using NUnit.Framework;
using BookDiary.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BookDiary.Tests.UnitTests.Models
{
    [TestFixture]
    public class ShelfBookTests
    {
        [Test]
        public void ShelfBook_IdProperty_ShouldHaveKeyAttribute()
        {
            var propertyInfo = typeof(ShelfBook).GetProperty("Id");

            var keyAttribute = propertyInfo.GetCustomAttributes(typeof(KeyAttribute), false).FirstOrDefault();

            Assert.IsNotNull(keyAttribute, "Id property should have KeyAttribute");
        }

        [Test]
        public void ShelfBook_IdProperty_ShouldHaveDatabaseGeneratedAttribute()
        {
            var propertyInfo = typeof(ShelfBook).GetProperty("Id");

            var dbGenAttribute = propertyInfo.GetCustomAttributes(typeof(DatabaseGeneratedAttribute), false).FirstOrDefault() as DatabaseGeneratedAttribute;

            Assert.IsNotNull(dbGenAttribute, "Id property should have DatabaseGeneratedAttribute");
            Assert.AreEqual(DatabaseGeneratedOption.Identity, dbGenAttribute.DatabaseGeneratedOption);
        }

        [Test]
        public void ShelfBook_BookIdProperty_ShouldHaveForeignKeyAttribute()
        {
            var propertyInfo = typeof(ShelfBook).GetProperty("BookId");

            var foreignKeyAttribute = propertyInfo.GetCustomAttributes(typeof(ForeignKeyAttribute), false).FirstOrDefault() as ForeignKeyAttribute;

            Assert.IsNotNull(foreignKeyAttribute, "BookId property should have ForeignKeyAttribute");
            Assert.AreEqual("Book", foreignKeyAttribute.Name);
        }

        [Test]
        public void ShelfBook_ShelfIdProperty_ShouldHaveForeignKeyAttribute()
        {
            var propertyInfo = typeof(ShelfBook).GetProperty("ShelfId");

            var foreignKeyAttribute = propertyInfo.GetCustomAttributes(typeof(ForeignKeyAttribute), false).FirstOrDefault() as ForeignKeyAttribute;

            Assert.IsNotNull(foreignKeyAttribute, "ShelfId property should have ForeignKeyAttribute");
            Assert.AreEqual("Shelf", foreignKeyAttribute.Name);
        }

        [TestCase(1, 10, 5)]
        [TestCase(2, 15, 7)]
        [TestCase(3, 20, 9)]
        public void ShelfBook_PropertiesSetCorrectly_ReturnsExpectedValues(int id, int bookId, int shelfId)
        {
            var book = new Book { Id = bookId, Title = $"Book {bookId}" };
            var shelf = new Shelf { Id = shelfId, Name = $"Shelf {shelfId}" };

            var shelfBook = new ShelfBook
            {
                Id = id,
                BookId = bookId,
                Book = book,
                ShelfId = shelfId,
                Shelf = shelf
            };

            Assert.AreEqual(id, shelfBook.Id);
            Assert.AreEqual(bookId, shelfBook.BookId);
            Assert.AreEqual(book, shelfBook.Book);
            Assert.AreEqual(shelfId, shelfBook.ShelfId);
            Assert.AreEqual(shelf, shelfBook.Shelf);
        }

        [Test]
        public void ShelfBook_NavigationProperties_SetCorrectly()
        {
            var book = new Book { Id = 1, Title = "Test Book" };
            var shelf = new Shelf { Id = 1, Name = "Test Shelf" };

            var shelfBook = new ShelfBook
            {
                Id = 1,
                BookId = 1,
                Book = book,
                ShelfId = 1,
                Shelf = shelf
            };

            Assert.AreEqual(book, shelfBook.Book);
            Assert.AreEqual(shelf, shelfBook.Shelf);

            var newBook = new Book { Id = 2, Title = "New Book" };
            var newShelf = new Shelf { Id = 2, Name = "New Shelf" };

            shelfBook.Book = newBook;
            shelfBook.Shelf = newShelf;

            Assert.AreEqual(newBook, shelfBook.Book);
            Assert.AreEqual(newShelf, shelfBook.Shelf);
        }

        [Test]
        public void ShelfBook_ForeignKeyProperties_UpdateCorrectly()
        {
            var shelfBook = new ShelfBook
            {
                Id = 1,
                BookId = 1,
                ShelfId = 1
            };

            shelfBook.BookId = 2;
            shelfBook.ShelfId = 2;

            Assert.AreEqual(2, shelfBook.BookId);
            Assert.AreEqual(2, shelfBook.ShelfId);
        }

        [Test]
        public void ShelfBook_BidirectionalRelationship_WorksCorrectly()
        {
            var book = new Book
            {
                Id = 1,
                Title = "Test Book",
                ShelfBooks = new List<ShelfBook>()
            };

            var shelf = new Shelf
            {
                Id = 1,
                Name = "Test Shelf",
                ShelfBooks = new List<ShelfBook>()
            };

            var shelfBook = new ShelfBook
            {
                Id = 1,
                BookId = 1,
                Book = book,
                ShelfId = 1,
                Shelf = shelf
            };

            book.ShelfBooks.Add(shelfBook);
            shelf.ShelfBooks.Add(shelfBook);

            Assert.AreEqual(1, book.ShelfBooks.Count);
            Assert.AreEqual(1, shelf.ShelfBooks.Count);
            Assert.IsTrue(book.ShelfBooks.Contains(shelfBook));
            Assert.IsTrue(shelf.ShelfBooks.Contains(shelfBook));

            var bookShelfBook = book.ShelfBooks.FirstOrDefault();
            var shelfShelfBook = shelf.ShelfBooks.FirstOrDefault();

            Assert.IsNotNull(bookShelfBook);
            Assert.IsNotNull(shelfShelfBook);
            Assert.AreEqual(bookShelfBook, shelfShelfBook);
            Assert.AreEqual(book, bookShelfBook.Book);
            Assert.AreEqual(shelf, shelfShelfBook.Shelf);
        }
    }
}
using NUnit.Framework;
using BookDiary.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BookDiary.Tests.UnitTests.Models
{
    [TestFixture]
    public class BookTagTests
    {
        [Test]
        public void BookTag_IdProperty_ShouldHaveKeyAttribute()
        {
            // Arrange
            var propertyInfo = typeof(BookTag).GetProperty("Id");

            // Act
            var keyAttribute = propertyInfo.GetCustomAttributes(typeof(KeyAttribute), false).FirstOrDefault();

            // Assert
            Assert.IsNotNull(keyAttribute, "Id property should have KeyAttribute");
        }

        [Test]
        public void BookTag_IdProperty_ShouldHaveDatabaseGeneratedAttribute()
        {
            // Arrange
            var propertyInfo = typeof(BookTag).GetProperty("Id");

            // Act
            var dbGenAttribute = propertyInfo.GetCustomAttributes(typeof(DatabaseGeneratedAttribute), false).FirstOrDefault() as DatabaseGeneratedAttribute;

            // Assert
            Assert.IsNotNull(dbGenAttribute, "Id property should have DatabaseGeneratedAttribute");
            Assert.AreEqual(DatabaseGeneratedOption.Identity, dbGenAttribute.DatabaseGeneratedOption);
        }

        [Test]
        public void BookTag_BookIdProperty_ShouldHaveForeignKeyAttribute()
        {
            // Arrange
            var propertyInfo = typeof(BookTag).GetProperty("BookId");

            // Act
            var foreignKeyAttribute = propertyInfo.GetCustomAttributes(typeof(ForeignKeyAttribute), false).FirstOrDefault() as ForeignKeyAttribute;

            // Assert
            Assert.IsNotNull(foreignKeyAttribute, "BookId property should have ForeignKeyAttribute");
            Assert.AreEqual("Book", foreignKeyAttribute.Name);
        }

        [Test]
        public void BookTag_TagIdProperty_ShouldHaveForeignKeyAttribute()
        {
            // Arrange
            var propertyInfo = typeof(BookTag).GetProperty("TagId");

            // Act
            var foreignKeyAttribute = propertyInfo.GetCustomAttributes(typeof(ForeignKeyAttribute), false).FirstOrDefault() as ForeignKeyAttribute;

            // Assert
            Assert.IsNotNull(foreignKeyAttribute, "TagId property should have ForeignKeyAttribute");
            Assert.AreEqual("Tag", foreignKeyAttribute.Name);
        }

        [TestCase(1, 10, 5)]
        [TestCase(2, 15, 7)]
        [TestCase(3, 20, 9)]
        public void BookTag_PropertiesSetCorrectly_ReturnsExpectedValues(int id, int bookId, int tagId)
        {
            // Arrange
            var book = new Book { Id = bookId, Title = $"Book {bookId}" };
            var tag = new Tag { Id = tagId, Name = $"Tag {tagId}" };

            var bookTag = new BookTag
            {
                Id = id,
                BookId = bookId,
                Book = book,
                TagId = tagId,
                Tag = tag
            };

            // Act & Assert
            Assert.AreEqual(id, bookTag.Id);
            Assert.AreEqual(bookId, bookTag.BookId);
            Assert.AreEqual(book, bookTag.Book);
            Assert.AreEqual(tagId, bookTag.TagId);
            Assert.AreEqual(tag, bookTag.Tag);
        }

        [Test]
        public void BookTag_NavigationProperties_SetCorrectly()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "Test Book" };
            var tag = new Tag { Id = 1, Name = "Test Tag" };

            var bookTag = new BookTag
            {
                Id = 1,
                BookId = 1,
                Book = book,
                TagId = 1,
                Tag = tag
            };

            // Act & Assert
            Assert.AreEqual(book, bookTag.Book);
            Assert.AreEqual(tag, bookTag.Tag);

            // Changing navigation properties should work
            var newBook = new Book { Id = 2, Title = "New Book" };
            var newTag = new Tag { Id = 2, Name = "New Tag" };

            bookTag.Book = newBook;
            bookTag.Tag = newTag;

            Assert.AreEqual(newBook, bookTag.Book);
            Assert.AreEqual(newTag, bookTag.Tag);
        }

        [Test]
        public void BookTag_ForeignKeyProperties_UpdateCorrectly()
        {
            // Arrange
            var bookTag = new BookTag
            {
                Id = 1,
                BookId = 1,
                TagId = 1
            };

            // Act
            bookTag.BookId = 2;
            bookTag.TagId = 2;

            // Assert
            Assert.AreEqual(2, bookTag.BookId);
            Assert.AreEqual(2, bookTag.TagId);
        }

        [Test]
        public void BookTag_BidirectionalRelationship_WorksCorrectly()
        {
            // Arrange - Create a book with BookTags collection
            var book = new Book
            {
                Id = 1,
                Title = "Test Book",
                BookTags = new List<BookTag>()
            };

            // Create a tag with BookTags collection
            var tag = new Tag
            {
                Id = 1,
                Name = "Test Tag",
                BookTags = new List<BookTag>()
            };

            // Create a BookTag that connects them
            var bookTag = new BookTag
            {
                Id = 1,
                BookId = 1,
                Book = book,
                TagId = 1,
                Tag = tag
            };

            // Act - Add the BookTag to both collections
            book.BookTags.Add(bookTag);
            tag.BookTags.Add(bookTag);

            // Assert - Verify the relationships are properly set
            Assert.AreEqual(1, book.BookTags.Count);
            Assert.AreEqual(1, tag.BookTags.Count);
            Assert.IsTrue(book.BookTags.Contains(bookTag));
            Assert.IsTrue(tag.BookTags.Contains(bookTag));

            // Verify that the navigation properties are set correctly
            var bookBookTag = book.BookTags.FirstOrDefault();
            var tagBookTag = tag.BookTags.FirstOrDefault();

            Assert.IsNotNull(bookBookTag);
            Assert.IsNotNull(tagBookTag);
            Assert.AreEqual(bookBookTag, tagBookTag);
            Assert.AreEqual(book, bookBookTag.Book);
            Assert.AreEqual(tag, tagBookTag.Tag);
        }

        [Test]
        public void BookTag_MultipleTagsForOneBook_WorksCorrectly()
        {
            // Arrange - Create a book with BookTags collection
            var book = new Book
            {
                Id = 1,
                Title = "Test Book",
                BookTags = new List<BookTag>()
            };

            // Create multiple tags
            var tag1 = new Tag { Id = 1, Name = "Fantasy", BookTags = new List<BookTag>() };
            var tag2 = new Tag { Id = 2, Name = "Adventure", BookTags = new List<BookTag>() };
            var tag3 = new Tag { Id = 3, Name = "Magic", BookTags = new List<BookTag>() };

            // Create BookTags to connect the book to multiple tags
            var bookTag1 = new BookTag { Id = 1, BookId = 1, Book = book, TagId = 1, Tag = tag1 };
            var bookTag2 = new BookTag { Id = 2, BookId = 1, Book = book, TagId = 2, Tag = tag2 };
            var bookTag3 = new BookTag { Id = 3, BookId = 1, Book = book, TagId = 3, Tag = tag3 };

            // Act - Add the BookTags to collections
            book.BookTags.Add(bookTag1);
            book.BookTags.Add(bookTag2);
            book.BookTags.Add(bookTag3);

            tag1.BookTags.Add(bookTag1);
            tag2.BookTags.Add(bookTag2);
            tag3.BookTags.Add(bookTag3);

            // Assert - Verify the book has multiple tags
            Assert.AreEqual(3, book.BookTags.Count);

            // Verify each tag is properly connected to the book
            var tagIds = book.BookTags.Select(bt => bt.TagId).ToList();
            Assert.Contains(1, tagIds);
            Assert.Contains(2, tagIds);
            Assert.Contains(3, tagIds);

            // Verify that each tag has the correct BookTag
            Assert.AreEqual(1, tag1.BookTags.Count);
            Assert.AreEqual(1, tag2.BookTags.Count);
            Assert.AreEqual(1, tag3.BookTags.Count);

            Assert.AreEqual(book, tag1.BookTags.First().Book);
            Assert.AreEqual(book, tag2.BookTags.First().Book);
            Assert.AreEqual(book, tag3.BookTags.First().Book);
        }

        [Test]
        public void BookTag_MultipleBooksForOneTag_WorksCorrectly()
        {
            // Arrange - Create a tag with BookTags collection
            var tag = new Tag
            {
                Id = 1,
                Name = "Fantasy",
                BookTags = new List<BookTag>()
            };

            // Create multiple books
            var book1 = new Book { Id = 1, Title = "Book 1", BookTags = new List<BookTag>() };
            var book2 = new Book { Id = 2, Title = "Book 2", BookTags = new List<BookTag>() };
            var book3 = new Book { Id = 3, Title = "Book 3", BookTags = new List<BookTag>() };

            // Create BookTags to connect the tag to multiple books
            var bookTag1 = new BookTag { Id = 1, BookId = 1, Book = book1, TagId = 1, Tag = tag };
            var bookTag2 = new BookTag { Id = 2, BookId = 2, Book = book2, TagId = 1, Tag = tag };
            var bookTag3 = new BookTag { Id = 3, BookId = 3, Book = book3, TagId = 1, Tag = tag };

            // Act - Add the BookTags to collections
            tag.BookTags.Add(bookTag1);
            tag.BookTags.Add(bookTag2);
            tag.BookTags.Add(bookTag3);

            book1.BookTags.Add(bookTag1);
            book2.BookTags.Add(bookTag2);
            book3.BookTags.Add(bookTag3);

            // Assert - Verify the tag has multiple books
            Assert.AreEqual(3, tag.BookTags.Count);

            // Verify each book is properly connected to the tag
            var bookIds = tag.BookTags.Select(bt => bt.BookId).ToList();
            Assert.Contains(1, bookIds);
            Assert.Contains(2, bookIds);
            Assert.Contains(3, bookIds);

            // Verify that each book has the correct BookTag
            Assert.AreEqual(1, book1.BookTags.Count);
            Assert.AreEqual(1, book2.BookTags.Count);
            Assert.AreEqual(1, book3.BookTags.Count);

            Assert.AreEqual(tag, book1.BookTags.First().Tag);
            Assert.AreEqual(tag, book2.BookTags.First().Tag);
            Assert.AreEqual(tag, book3.BookTags.First().Tag);
        }
    }
}
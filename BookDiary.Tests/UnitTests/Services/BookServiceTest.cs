using NUnit.Framework;
using BookDiary.Core.Services;
using BookDiary.Core.IServices;
using BookDiary.DataAccess.Repository;
using BookDiary.Models;
using Moq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookDiary.Tests.UnitTests.Services
{
    [TestFixture]
    public class BookServiceTests
    {
        private Mock<IRepository<Book>> _mockBookRepo;
        private Mock<IRepository<BookTag>> _mockBookTagRepo;
        private Mock<IRepository<Tag>> _mockTagRepo;
        private IBookService _bookService;

        [SetUp]
        public void Setup()
        {
            _mockBookRepo = new Mock<IRepository<Book>>();
            _mockBookTagRepo = new Mock<IRepository<BookTag>>();
            _mockTagRepo = new Mock<IRepository<Tag>>();
            _bookService = new BookService(_mockBookRepo.Object, _mockBookTagRepo.Object, _mockTagRepo.Object);
        }
        [Test]
        public void BookService_Constructor_InitializesRepositories()
        {
            // Arrange
            var bookRepo = new Mock<IRepository<Book>>();
            var bookTagRepo = new Mock<IRepository<BookTag>>();
            var tagRepo = new Mock<IRepository<Tag>>();

            // Act
            var bookService = new BookService(bookRepo.Object, bookTagRepo.Object, tagRepo.Object);

            // Assert - Indirect verification since we can't access private fields
            Assert.DoesNotThrowAsync(async () =>
            {
                bookRepo.Setup(r => r.GetAll()).Returns(new List<Book>().AsQueryable());
                var result = bookService.GetAll();
            });
        }
        [Test]
        public async Task GetById_ShouldCallRepositoryWithCorrectId()
        {
            // Arrange
            int bookId = 1;
            var expectedBook = new Book { Id = bookId, Title = "Test Book" };
            _mockBookRepo.Setup(r => r.GetById(bookId)).ReturnsAsync(expectedBook);

            // Act
            var result = await _bookService.GetById(bookId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedBook));
            _mockBookRepo.Verify(r => r.GetById(bookId), Times.Once);
        }

        [Test]
        public void GetAll_ShouldReturnAllBooks()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { Id = 1, Title = "Book 1" },
                new Book { Id = 2, Title = "Book 2" },
                new Book { Id = 3, Title = "Book 3" }
            }.AsQueryable();

            _mockBookRepo.Setup(r => r.GetAll()).Returns(books);

            // Act
            var result = _bookService.GetAll();

            // Assert
            Assert.That(result, Is.EqualTo(books));
            _mockBookRepo.Verify(r => r.GetAll(), Times.Once);
        }

        [Test]
        public async Task Find_ShouldCallRepositoryWithCorrectFilter()
        {
            // Arrange
            var expectedBooks = new List<Book>
            {
                new Book { Id = 1, Title = "Book 1" },
                new Book { Id = 2, Title = "Book 2" }
            };

            Expression<Func<Book, bool>> filter = b => b.Title.Contains("Book");

            _mockBookRepo.Setup(r => r.Find(It.IsAny<Expression<Func<Book, bool>>>()))
                    .ReturnsAsync(expectedBooks);

            // Act
            var result = await _bookService.Find(filter);

            // Assert
            Assert.That(result, Is.EqualTo(expectedBooks));
            _mockBookRepo.Verify(r => r.Find(It.IsAny<Expression<Func<Book, bool>>>()), Times.Once);
        }

        [Test]
        public async Task Add_ShouldCallRepositoryAddMethod()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "New Book" };
            _mockBookRepo.Setup(r => r.Add(It.IsAny<Book>())).Returns(Task.CompletedTask);

            // Act
            await _bookService.Add(book);

            // Assert
            _mockBookRepo.Verify(r => r.Add(book), Times.Once);
        }

        [Test]
        public async Task Update_ShouldCallRepositoryUpdateMethod()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "Updated Book" };
            _mockBookRepo.Setup(r => r.Update(It.IsAny<Book>())).Returns(Task.CompletedTask);

            // Act
            await _bookService.Update(book);

            // Assert
            _mockBookRepo.Verify(r => r.Update(book), Times.Once);
        }

        [Test]
        public async Task Delete_ShouldCallRepositoryDeleteMethod()
        {
            // Arrange
            int bookId = 1;
            _mockBookRepo.Setup(r => r.Delete(bookId)).Returns(Task.CompletedTask);

            // Act
            await _bookService.Delete(bookId);

            // Assert
            _mockBookRepo.Verify(r => r.Delete(bookId), Times.Once);
        }

        [Test]
        public void Get_ShouldThrowNotImplementedException()
        {
            // Arrange
            Expression<Func<Book, bool>> filter = b => b.Id == 1;

            // Act & Assert
            Assert.ThrowsAsync<NotImplementedException>(async () => await _bookService.Get(filter));
        }

        [Test]
        public void AddTagToBook_ShouldThrowNotImplementedException()
        {
            // Arrange
            string bookName = "Test Book";
            string tagName = "Fiction";

            // Act & Assert
            Assert.ThrowsAsync<NotImplementedException>(async () => await _bookService.AddTagToBook(bookName, tagName));
        }

        [Test]
        public void Constructor_ShouldSetRepositoryProperties()
        {
            // Arrange
            var bookRepo = new Mock<IRepository<Book>>();
            var bookTagRepo = new Mock<IRepository<BookTag>>();
            var tagRepo = new Mock<IRepository<Tag>>();

            // Act
            var bookService = new BookService(bookRepo.Object, bookTagRepo.Object, tagRepo.Object);

            // Assert - this is testing private fields, so we need to verify indirectly 
            // by checking that repository methods can be called without throwing exceptions
            Assert.DoesNotThrowAsync(async () =>
            {
                bookRepo.Setup(r => r.GetAll()).Returns(new List<Book>().AsQueryable());
                var result = bookService.GetAll();
            });
        }

        [Test]
        public void BookService_WithNullRepository_ShouldThrowArgumentNullException()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentNullException>(() => new BookService(null, _mockBookTagRepo.Object, _mockTagRepo.Object));
            Assert.Throws<ArgumentNullException>(() => new BookService(_mockBookRepo.Object, null, _mockTagRepo.Object));
            Assert.Throws<ArgumentNullException>(() => new BookService(_mockBookRepo.Object, _mockBookTagRepo.Object, null));
        }
    }
}
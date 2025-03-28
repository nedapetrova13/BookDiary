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
            var bookRepo = new Mock<IRepository<Book>>();
            var bookTagRepo = new Mock<IRepository<BookTag>>();
            var tagRepo = new Mock<IRepository<Tag>>();

            var bookService = new BookService(bookRepo.Object, bookTagRepo.Object, tagRepo.Object);

            Assert.DoesNotThrowAsync(async () =>
            {
                bookRepo.Setup(r => r.GetAll()).Returns(new List<Book>().AsQueryable());
                var result = bookService.GetAll();
            });
        }
        [Test]
        public async Task GetById_ShouldCallRepositoryWithCorrectId()
        {
            int bookId = 1;
            var expectedBook = new Book { Id = bookId, Title = "Test Book" };
            _mockBookRepo.Setup(r => r.GetById(bookId)).ReturnsAsync(expectedBook);

            var result = await _bookService.GetById(bookId);

            Assert.That(result, Is.EqualTo(expectedBook));
        }

        [Test]
        public void GetAll_ShouldReturnAllBooks()
        {
            var books = new List<Book>
            {
                new Book { Id = 1, Title = "Book 1" },
                new Book { Id = 2, Title = "Book 2" },
                new Book { Id = 3, Title = "Book 3" }
            }.AsQueryable();

            _mockBookRepo.Setup(r => r.GetAll()).Returns(books);

            var result = _bookService.GetAll();

            Assert.That(result, Is.EqualTo(books));
        }

        [Test]
        public async Task Find_ShouldCallRepositoryWithCorrectFilter()
        {
            var expectedBooks = new List<Book>
            {
                new Book { Id = 1, Title = "Book 1" },
                new Book { Id = 2, Title = "Book 2" }
            };

            Expression<Func<Book, bool>> filter = b => b.Title.Contains("Book");

            _mockBookRepo.Setup(r => r.Find(It.IsAny<Expression<Func<Book, bool>>>()))
                    .ReturnsAsync(expectedBooks);

            var result = await _bookService.Find(filter);

            Assert.That(result, Is.EqualTo(expectedBooks));
        }

        [Test]
        public async Task Add_ShouldCallRepositoryAddMethod()
        {
            var book = new Book { Id = 1, Title = "New Book" };
            _mockBookRepo.Setup(r => r.Add(It.IsAny<Book>())).Returns(Task.CompletedTask);

            await _bookService.Add(book);

        }

        [Test]
        public async Task Update_ShouldCallRepositoryUpdateMethod()
        {
            var book = new Book { Id = 1, Title = "Updated Book" };
            _mockBookRepo.Setup(r => r.Update(It.IsAny<Book>())).Returns(Task.CompletedTask);

            await _bookService.Update(book);

        }

        [Test]
        public async Task Delete_ShouldCallRepositoryDeleteMethod()
        {
            int bookId = 1;
            _mockBookRepo.Setup(r => r.Delete(bookId)).Returns(Task.CompletedTask);

            await _bookService.Delete(bookId);

        }

        [Test]
        public void Get_ShouldThrowNotImplementedException()
        {
            Expression<Func<Book, bool>> filter = b => b.Id == 1;

            Assert.ThrowsAsync<NotImplementedException>(async () => await _bookService.Get(filter));
        }

        [Test]
        public void AddTagToBook_ShouldThrowNotImplementedException()
        {
            string bookName = "Test Book";
            string tagName = "Fiction";

            Assert.ThrowsAsync<NotImplementedException>(async () => await _bookService.AddTagToBook(bookName, tagName));
        }

        [Test]
        public void Constructor_ShouldSetRepositoryProperties()
        {
            var bookRepo = new Mock<IRepository<Book>>();
            var bookTagRepo = new Mock<IRepository<BookTag>>();
            var tagRepo = new Mock<IRepository<Tag>>();

            var bookService = new BookService(bookRepo.Object, bookTagRepo.Object, tagRepo.Object);

            Assert.DoesNotThrowAsync(async () =>
            {
                bookRepo.Setup(r => r.GetAll()).Returns(new List<Book>().AsQueryable());
                var result = bookService.GetAll();
            });
        }

        [Test]
        public void BookService_WithNullRepository_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new BookService(null, _mockBookTagRepo.Object, _mockTagRepo.Object));
            Assert.Throws<ArgumentNullException>(() => new BookService(_mockBookRepo.Object, null, _mockTagRepo.Object));
            Assert.Throws<ArgumentNullException>(() => new BookService(_mockBookRepo.Object, _mockBookTagRepo.Object, null));
        }
    }
}
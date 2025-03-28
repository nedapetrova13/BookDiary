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
    public class ShelfBookServiceTests
    {
        private Mock<IRepository<ShelfBook>> _mockRepo;
        private IShelfBookService _shelfBookService;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IRepository<ShelfBook>>();
            _shelfBookService = new ShelfBookService(_mockRepo.Object);
        }

        [Test]
        public async Task GetById_ShouldCallRepositoryWithCorrectId()
        {
            int shelfBookId = 1;
            var expectedShelfBook = new ShelfBook { Id = shelfBookId, BookId = 10, ShelfId = 5 };
            _mockRepo.Setup(r => r.GetById(shelfBookId)).ReturnsAsync(expectedShelfBook);

            var result = await _shelfBookService.GetById(shelfBookId);

            Assert.That(result, Is.EqualTo(expectedShelfBook));
        }

        [Test]
        public void GetAll_ShouldReturnAllShelfBooks()
        {
            var shelfBooks = new List<ShelfBook>
            {
                new ShelfBook { Id = 1, BookId = 10, ShelfId = 5 },
                new ShelfBook { Id = 2, BookId = 11, ShelfId = 5 },
                new ShelfBook { Id = 3, BookId = 10, ShelfId = 6 }
            }.AsQueryable();

            _mockRepo.Setup(r => r.GetAll()).Returns(shelfBooks);

            var result = _shelfBookService.GetAll();

            Assert.That(result, Is.EqualTo(shelfBooks));
        }

        [Test]
        public async Task Get_ShouldCallRepositoryWithCorrectFilter()
        {
            var expectedShelfBook = new ShelfBook { Id = 1, BookId = 10, ShelfId = 5 };
            Expression<Func<ShelfBook, bool>> filter = sb => sb.BookId == 10 && sb.ShelfId == 5;

            _mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<ShelfBook, bool>>>()))
                    .ReturnsAsync(expectedShelfBook);

            var result = await _shelfBookService.Get(filter);

            Assert.That(result, Is.EqualTo(expectedShelfBook));
        }

        [Test]
        public async Task Find_ShouldCallRepositoryWithCorrectFilter()
        {
            var expectedShelfBooks = new List<ShelfBook>
            {
                new ShelfBook { Id = 1, BookId = 10, ShelfId = 5 },
                new ShelfBook { Id = 3, BookId = 10, ShelfId = 6 }
            };

            Expression<Func<ShelfBook, bool>> filter = sb => sb.BookId == 10;

            _mockRepo.Setup(r => r.Find(It.IsAny<Expression<Func<ShelfBook, bool>>>()))
                    .ReturnsAsync(expectedShelfBooks);

            var result = await _shelfBookService.Find(filter);

            Assert.That(result, Is.EqualTo(expectedShelfBooks));
        }

        [Test]
        public async Task Add_ShouldCallRepositoryAddMethod()
        {
            var shelfBook = new ShelfBook { BookId = 10, ShelfId = 5 };
            _mockRepo.Setup(r => r.Add(It.IsAny<ShelfBook>())).Returns(Task.CompletedTask);

            await _shelfBookService.Add(shelfBook);

        }

        [Test]
        public async Task Update_ShouldCallRepositoryUpdateMethod()
        {
            var shelfBook = new ShelfBook { Id = 1, BookId = 10, ShelfId = 5 };
            _mockRepo.Setup(r => r.Update(It.IsAny<ShelfBook>())).Returns(Task.CompletedTask);

            await _shelfBookService.Update(shelfBook);
        }

        [Test]
        public async Task Delete_ShouldCallRepositoryDeleteMethod()
        {
            int shelfBookId = 1;
            _mockRepo.Setup(r => r.Delete(shelfBookId)).Returns(Task.CompletedTask);

            await _shelfBookService.Delete(shelfBookId);
        }

        [Test]
        public async Task DeleteShelfBook_ShouldCallRepositoryDeleteMappingMethod()
        {
            int bookId = 10;
            int shelfId = 5;

            _mockRepo.Setup(r => r.DeleteMapping<ShelfBook>(
                It.IsAny<Expression<Func<ShelfBook, bool>>>()))
                .Returns(Task.CompletedTask);

            await _shelfBookService.DeleteShelfBook(bookId, shelfId);
        }

        [Test]
        public void Constructor_WithNullRepository_ShouldNotThrowException()
        {
            Assert.DoesNotThrow(() => new ShelfBookService(null));

        }

        [Test]
        public void Constructor_WithValidRepository_ShouldInitializeService()
        {
            var repo = new Mock<IRepository<ShelfBook>>();

            var service = new ShelfBookService(repo.Object);

            Assert.DoesNotThrow(() => service.GetAll());
        }

        [Test]
        public async Task ShelfBookWorkflow_AddFindDeleteMapping_ShouldWorkCorrectly()
        {
            int bookId = 10;
            int shelfId = 5;

            var shelfBook = new ShelfBook
            {
                Id = 1,
                BookId = bookId,
                ShelfId = shelfId
            };

            _mockRepo.Setup(r => r.Add(It.IsAny<ShelfBook>())).Returns(Task.CompletedTask);
            _mockRepo.Setup(r => r.Find(It.IsAny<Expression<Func<ShelfBook, bool>>>()))
                .ReturnsAsync(new List<ShelfBook> { shelfBook });
            _mockRepo.Setup(r => r.DeleteMapping<ShelfBook>(
                It.IsAny<Expression<Func<ShelfBook, bool>>>()))
                .Returns(Task.CompletedTask);

            await _shelfBookService.Add(shelfBook);
\
            var foundShelfBooks = await _shelfBookService.Find(sb => sb.BookId == bookId && sb.ShelfId == shelfId);
            Assert.That(foundShelfBooks.Count, Is.EqualTo(1));
            Assert.That(foundShelfBooks[0], Is.EqualTo(shelfBook));

            await _shelfBookService.DeleteShelfBook(bookId, shelfId);
            _mockRepo.Verify(r => r.DeleteMapping<ShelfBook>(It.IsAny<Expression<Func<ShelfBook, bool>>>()), Times.Once);
        }

        [Test]
        public async Task ShelfBook_WithNavigationProperties_ShouldBeRetrieved()
        {
            var shelf = new Shelf { Id = 5, Name = "Favorites" };
            var book = new Book { Id = 10, Title = "Sample Book" };

            var shelfBook = new ShelfBook
            {
                Id = 1,
                BookId = 10,
                ShelfId = 5,
                Book = book,
                Shelf = shelf
            };

            _mockRepo.Setup(r => r.GetById(shelfBook.Id)).ReturnsAsync(shelfBook);

            var result = await _shelfBookService.GetById(shelfBook.Id);

            Assert.That(result, Is.EqualTo(shelfBook));
            Assert.That(result.Book, Is.Not.Null);
            Assert.That(result.Book.Id, Is.EqualTo(10));
            Assert.That(result.Book.Title, Is.EqualTo("Sample Book"));
            Assert.That(result.Shelf, Is.Not.Null);
            Assert.That(result.Shelf.Id, Is.EqualTo(5));
            Assert.That(result.Shelf.Name, Is.EqualTo("Favorites"));
        }
    }
}
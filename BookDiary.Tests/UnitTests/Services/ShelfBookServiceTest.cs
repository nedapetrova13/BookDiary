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
            // Arrange
            int shelfBookId = 1;
            var expectedShelfBook = new ShelfBook { Id = shelfBookId, BookId = 10, ShelfId = 5 };
            _mockRepo.Setup(r => r.GetById(shelfBookId)).ReturnsAsync(expectedShelfBook);

            // Act
            var result = await _shelfBookService.GetById(shelfBookId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedShelfBook));
            _mockRepo.Verify(r => r.GetById(shelfBookId), Times.Once);
        }

        [Test]
        public void GetAll_ShouldReturnAllShelfBooks()
        {
            // Arrange
            var shelfBooks = new List<ShelfBook>
            {
                new ShelfBook { Id = 1, BookId = 10, ShelfId = 5 },
                new ShelfBook { Id = 2, BookId = 11, ShelfId = 5 },
                new ShelfBook { Id = 3, BookId = 10, ShelfId = 6 }
            }.AsQueryable();

            _mockRepo.Setup(r => r.GetAll()).Returns(shelfBooks);

            // Act
            var result = _shelfBookService.GetAll();

            // Assert
            Assert.That(result, Is.EqualTo(shelfBooks));
            _mockRepo.Verify(r => r.GetAll(), Times.Once);
        }

        [Test]
        public async Task Get_ShouldCallRepositoryWithCorrectFilter()
        {
            // Arrange
            var expectedShelfBook = new ShelfBook { Id = 1, BookId = 10, ShelfId = 5 };
            Expression<Func<ShelfBook, bool>> filter = sb => sb.BookId == 10 && sb.ShelfId == 5;

            _mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<ShelfBook, bool>>>()))
                    .ReturnsAsync(expectedShelfBook);

            // Act
            var result = await _shelfBookService.Get(filter);

            // Assert
            Assert.That(result, Is.EqualTo(expectedShelfBook));
            _mockRepo.Verify(r => r.Get(It.IsAny<Expression<Func<ShelfBook, bool>>>()), Times.Once);
        }

        [Test]
        public async Task Find_ShouldCallRepositoryWithCorrectFilter()
        {
            // Arrange
            var expectedShelfBooks = new List<ShelfBook>
            {
                new ShelfBook { Id = 1, BookId = 10, ShelfId = 5 },
                new ShelfBook { Id = 3, BookId = 10, ShelfId = 6 }
            };

            Expression<Func<ShelfBook, bool>> filter = sb => sb.BookId == 10;

            _mockRepo.Setup(r => r.Find(It.IsAny<Expression<Func<ShelfBook, bool>>>()))
                    .ReturnsAsync(expectedShelfBooks);

            // Act
            var result = await _shelfBookService.Find(filter);

            // Assert
            Assert.That(result, Is.EqualTo(expectedShelfBooks));
            _mockRepo.Verify(r => r.Find(It.IsAny<Expression<Func<ShelfBook, bool>>>()), Times.Once);
        }

        [Test]
        public async Task Add_ShouldCallRepositoryAddMethod()
        {
            // Arrange
            var shelfBook = new ShelfBook { BookId = 10, ShelfId = 5 };
            _mockRepo.Setup(r => r.Add(It.IsAny<ShelfBook>())).Returns(Task.CompletedTask);

            // Act
            await _shelfBookService.Add(shelfBook);

            // Assert
            _mockRepo.Verify(r => r.Add(shelfBook), Times.Once);
        }

        [Test]
        public async Task Update_ShouldCallRepositoryUpdateMethod()
        {
            // Arrange
            var shelfBook = new ShelfBook { Id = 1, BookId = 10, ShelfId = 5 };
            _mockRepo.Setup(r => r.Update(It.IsAny<ShelfBook>())).Returns(Task.CompletedTask);

            // Act
            await _shelfBookService.Update(shelfBook);

            // Assert
            _mockRepo.Verify(r => r.Update(shelfBook), Times.Once);
        }

        [Test]
        public async Task Delete_ShouldCallRepositoryDeleteMethod()
        {
            // Arrange
            int shelfBookId = 1;
            _mockRepo.Setup(r => r.Delete(shelfBookId)).Returns(Task.CompletedTask);

            // Act
            await _shelfBookService.Delete(shelfBookId);

            // Assert
            _mockRepo.Verify(r => r.Delete(shelfBookId), Times.Once);
        }

        [Test]
        public async Task DeleteShelfBook_ShouldCallRepositoryDeleteMappingMethod()
        {
            // Arrange
            int bookId = 10;
            int shelfId = 5;

            _mockRepo.Setup(r => r.DeleteMapping<ShelfBook>(
                It.IsAny<Expression<Func<ShelfBook, bool>>>()))
                .Returns(Task.CompletedTask);

            // Act
            await _shelfBookService.DeleteShelfBook(bookId, shelfId);

            // Assert
            _mockRepo.Verify(r => r.DeleteMapping<ShelfBook>(
                It.IsAny<Expression<Func<ShelfBook, bool>>>()), Times.Once);
        }

        [Test]
        public void Constructor_WithNullRepository_ShouldNotThrowException()
        {
            // Arrange & Act & Assert
            // This test verifies the current behavior, which does not throw when null is passed
            Assert.DoesNotThrow(() => new ShelfBookService(null));

            // Note: If you want to add null checking to your constructor, this test should be changed to:
            // Assert.Throws<ArgumentNullException>(() => new ShelfBookService(null));
        }

        [Test]
        public void Constructor_WithValidRepository_ShouldInitializeService()
        {
            // Arrange
            var repo = new Mock<IRepository<ShelfBook>>();

            // Act
            var service = new ShelfBookService(repo.Object);

            // Assert - test that the service is properly initialized by calling a method
            Assert.DoesNotThrow(() => service.GetAll());
        }

        [Test]
        public async Task ShelfBookWorkflow_AddFindDeleteMapping_ShouldWorkCorrectly()
        {
            // Arrange
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

            // Act & Assert - Full workflow
            // 1. Add a shelf-book mapping
            await _shelfBookService.Add(shelfBook);
            _mockRepo.Verify(r => r.Add(shelfBook), Times.Once);

            // 2. Find the mapping
            var foundShelfBooks = await _shelfBookService.Find(sb => sb.BookId == bookId && sb.ShelfId == shelfId);
            Assert.That(foundShelfBooks.Count, Is.EqualTo(1));
            Assert.That(foundShelfBooks[0], Is.EqualTo(shelfBook));

            // 3. Delete the mapping using DeleteShelfBook
            await _shelfBookService.DeleteShelfBook(bookId, shelfId);
            _mockRepo.Verify(r => r.DeleteMapping<ShelfBook>(It.IsAny<Expression<Func<ShelfBook, bool>>>()), Times.Once);
        }

        [Test]
        public async Task ShelfBook_WithNavigationProperties_ShouldBeRetrieved()
        {
            // Arrange
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

            // Act
            var result = await _shelfBookService.GetById(shelfBook.Id);

            // Assert
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
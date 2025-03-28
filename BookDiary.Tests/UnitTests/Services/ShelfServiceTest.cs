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
    public class ShelfServiceTests
    {
        private Mock<IRepository<Shelf>> _mockRepo;
        private IShelfService _shelfService;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IRepository<Shelf>>();
            _shelfService = new ShelfService(_mockRepo.Object);
        }

        [Test]
        public async Task GetById_ShouldCallRepositoryWithCorrectId()
        {
            int shelfId = 1;
            var expectedShelf = new Shelf { Id = shelfId, Name = "Favorites" };
            _mockRepo.Setup(r => r.GetById(shelfId)).ReturnsAsync(expectedShelf);

            var result = await _shelfService.GetById(shelfId);

            Assert.That(result, Is.EqualTo(expectedShelf));
        }

        [Test]
        public void GetAll_ShouldReturnAllShelves()
        {
            var shelves = new List<Shelf>
            {
                new Shelf { Id = 1, Name = "Favorites" },
                new Shelf { Id = 2, Name = "To Read" },
                new Shelf { Id = 3, Name = "Reading Now" }
            }.AsQueryable();

            _mockRepo.Setup(r => r.GetAll()).Returns(shelves);

            var result = _shelfService.GetAll();

            Assert.That(result, Is.EqualTo(shelves));
        }

        [Test]
        public async Task Get_ShouldCallRepositoryWithCorrectFilter()
        {
            var expectedShelf = new Shelf { Id = 1, Name = "Favorites" };
            Expression<Func<Shelf, bool>> filter = s => s.Name == "Favorites";

            _mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<Shelf, bool>>>()))
                    .ReturnsAsync(expectedShelf);

            var result = await _shelfService.Get(filter);

            Assert.That(result, Is.EqualTo(expectedShelf));
        }

        [Test]
        public async Task Find_ShouldCallRepositoryWithCorrectFilter()
        {
            var expectedShelves = new List<Shelf>
            {
                new Shelf { Id = 1, Name = "Favorites" },
                new Shelf { Id = 4, Name = "All Time Favorites" }
            };

            Expression<Func<Shelf, bool>> filter = s => s.Name.Contains("Favorites");

            _mockRepo.Setup(r => r.Find(It.IsAny<Expression<Func<Shelf, bool>>>()))
                    .ReturnsAsync(expectedShelves);

            var result = await _shelfService.Find(filter);

            Assert.That(result, Is.EqualTo(expectedShelves));
        }

        [Test]
        public async Task Add_ShouldCallRepositoryAddMethod()
        {
            var shelf = new Shelf { Name = "New Shelf" };
            _mockRepo.Setup(r => r.Add(It.IsAny<Shelf>())).Returns(Task.CompletedTask);

            await _shelfService.Add(shelf);
        }

        [Test]
        public async Task Update_ShouldCallRepositoryUpdateMethod()
        {
            var shelf = new Shelf { Id = 1, Name = "Updated Shelf" };
            _mockRepo.Setup(r => r.Update(It.IsAny<Shelf>())).Returns(Task.CompletedTask);

            await _shelfService.Update(shelf);
        }

        [Test]
        public async Task Delete_ShouldCallRepositoryDeleteMethod()
        {
            int shelfId = 1;
            _mockRepo.Setup(r => r.Delete(shelfId)).Returns(Task.CompletedTask);

            await _shelfService.Delete(shelfId);
        }

        [Test]
        public void Constructor_WithNullRepository_ShouldNotThrowException()
        {
            Assert.DoesNotThrow(() => new ShelfService(null));

        }

        [Test]
        public void Constructor_WithValidRepository_ShouldInitializeService()
        {
            var repo = new Mock<IRepository<Shelf>>();

            var service = new ShelfService(repo.Object);

            Assert.DoesNotThrow(() => service.GetAll());
        }

        [Test]
        public async Task ShelfWithBooks_ShouldWorkCorrectly()
        {
            var shelf = new Shelf
            {
                Id = 1,
                Name = "Test Shelf",
                ShelfBooks = new List<ShelfBook>
                {
                    new ShelfBook
                    {
                        Id = 1,
                        ShelfId = 1,
                        BookId = 10,
                        Book = new Book { Id = 10, Title = "Book 1" }
                    },
                    new ShelfBook
                    {
                        Id = 2,
                        ShelfId = 1,
                        BookId = 20,
                        Book = new Book { Id = 20, Title = "Book 2" }
                    }
                }
            };

            _mockRepo.Setup(r => r.GetById(shelf.Id)).ReturnsAsync(shelf);

            var result = await _shelfService.GetById(shelf.Id);

            Assert.That(result, Is.EqualTo(shelf));
            Assert.That(result.ShelfBooks.Count, Is.EqualTo(2));
            Assert.That(result.ShelfBooks.Any(sb => sb.BookId == 10 && sb.Book.Title == "Book 1"), Is.True);
            Assert.That(result.ShelfBooks.Any(sb => sb.BookId == 20 && sb.Book.Title == "Book 2"), Is.True);
        }

        [Test]
        public async Task ShelfWorkflow_AddFindUpdateDelete_ShouldWorkCorrectly()
        {
            var userId = "user123";
            var shelf = new Shelf
            {
                Id = 1,
                Name = "Reading List",
                UserId = userId
            };

            var updatedShelf = new Shelf
            {
                Id = 1,
                Name = "My Reading List", 
                UserId = userId
            };

            _mockRepo.Setup(r => r.Add(It.IsAny<Shelf>())).Returns(Task.CompletedTask);
            _mockRepo.Setup(r => r.Find(It.IsAny<Expression<Func<Shelf, bool>>>()))
                .ReturnsAsync(new List<Shelf> { shelf });
            _mockRepo.Setup(r => r.Update(It.IsAny<Shelf>())).Returns(Task.CompletedTask);
            _mockRepo.Setup(r => r.Delete(It.IsAny<int>())).Returns(Task.CompletedTask);

            await _shelfService.Add(shelf);

            var foundShelves = await _shelfService.Find(s => s.UserId == userId);
            Assert.That(foundShelves.Count, Is.EqualTo(1));
            Assert.That(foundShelves[0], Is.EqualTo(shelf));

            await _shelfService.Update(updatedShelf);
            _mockRepo.Verify(r => r.Update(updatedShelf), Times.Once);

            await _shelfService.Delete(shelf.Id);
            _mockRepo.Verify(r => r.Delete(shelf.Id), Times.Once);
        }
    }
}
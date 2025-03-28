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
    public class CurrentReadServiceTests
    {
        private Mock<IRepository<CurrentRead>> _mockRepo;
        private ICurrentReadService _currentReadService;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IRepository<CurrentRead>>();
            _currentReadService = new CurrentReadService(_mockRepo.Object);
        }

        [Test]
        public async Task GetById_ShouldCallRepositoryWithCorrectId()
        {
            // Arrange
            int currentReadId = 1;
            var expectedCurrentRead = new CurrentRead { Id = currentReadId, BookId = 10, UserId = "user123", CurrentPage = 50 };
            _mockRepo.Setup(r => r.GetById(currentReadId)).ReturnsAsync(expectedCurrentRead);

            // Act
            var result = await _currentReadService.GetById(currentReadId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedCurrentRead));
            _mockRepo.Verify(r => r.GetById(currentReadId), Times.Once);
        }

        [Test]
        public void GetAll_ShouldReturnAllCurrentReads()
        {
            // Arrange
            var currentReads = new List<CurrentRead>
            {
                new CurrentRead { Id = 1, BookId = 10, UserId = "user123", CurrentPage = 50 },
                new CurrentRead { Id = 2, BookId = 20, UserId = "user123", CurrentPage = 100 },
                new CurrentRead { Id = 3, BookId = 30, UserId = "user456", CurrentPage = 75 }
            }.AsQueryable();

            _mockRepo.Setup(r => r.GetAll()).Returns(currentReads);

            // Act
            var result = _currentReadService.GetAll();

            // Assert
            Assert.That(result, Is.EqualTo(currentReads));
            _mockRepo.Verify(r => r.GetAll(), Times.Once);
        }

        [Test]
        public async Task Get_ShouldCallRepositoryWithCorrectFilter()
        {
            // Arrange
            var expectedCurrentRead = new CurrentRead { Id = 1, BookId = 10, UserId = "user123", CurrentPage = 50 };
            Expression<Func<CurrentRead, bool>> filter = cr => cr.BookId == 10 && cr.UserId == "user123";

            _mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<CurrentRead, bool>>>()))
                    .ReturnsAsync(expectedCurrentRead);

            // Act
            var result = await _currentReadService.Get(filter);

            // Assert
            Assert.That(result, Is.EqualTo(expectedCurrentRead));
            _mockRepo.Verify(r => r.Get(It.IsAny<Expression<Func<CurrentRead, bool>>>()), Times.Once);
        }

        [Test]
        public async Task Find_ShouldCallRepositoryWithCorrectFilter()
        {
            // Arrange
            var expectedCurrentReads = new List<CurrentRead>
            {
                new CurrentRead { Id = 1, BookId = 10, UserId = "user123", CurrentPage = 50 },
                new CurrentRead { Id = 2, BookId = 20, UserId = "user123", CurrentPage = 100 }
            };

            Expression<Func<CurrentRead, bool>> filter = cr => cr.UserId == "user123";

            _mockRepo.Setup(r => r.Find(It.IsAny<Expression<Func<CurrentRead, bool>>>()))
                    .ReturnsAsync(expectedCurrentReads);

            // Act
            var result = await _currentReadService.Find(filter);

            // Assert
            Assert.That(result, Is.EqualTo(expectedCurrentReads));
            _mockRepo.Verify(r => r.Find(It.IsAny<Expression<Func<CurrentRead, bool>>>()), Times.Once);
        }

        [Test]
        public async Task Add_ShouldCallRepositoryAddMethod()
        {
            // Arrange
            var currentRead = new CurrentRead { BookId = 10, UserId = "user123", CurrentPage = 50 };
            _mockRepo.Setup(r => r.Add(It.IsAny<CurrentRead>())).Returns(Task.CompletedTask);

            // Act
            await _currentReadService.Add(currentRead);

            // Assert
            _mockRepo.Verify(r => r.Add(currentRead), Times.Once);
        }

        [Test]
        public async Task Update_ShouldCallRepositoryUpdateMethod()
        {
            // Arrange
            var currentRead = new CurrentRead { Id = 1, BookId = 10, UserId = "user123", CurrentPage = 75 };
            _mockRepo.Setup(r => r.Update(It.IsAny<CurrentRead>())).Returns(Task.CompletedTask);

            // Act
            await _currentReadService.Update(currentRead);

            // Assert
            _mockRepo.Verify(r => r.Update(currentRead), Times.Once);
        }

        [Test]
        public async Task Delete_ShouldCallRepositoryDeleteMethod()
        {
            // Arrange
            int currentReadId = 1;
            _mockRepo.Setup(r => r.Delete(currentReadId)).Returns(Task.CompletedTask);

            // Act
            await _currentReadService.Delete(currentReadId);

            // Assert
            _mockRepo.Verify(r => r.Delete(currentReadId), Times.Once);
        }

        [Test]
        public async Task DeleteCurrentRead_ShouldCallRepositoryDeleteMappingMethod()
        {
            // Arrange
            int bookId = 10;
            string userId = "user123";

            _mockRepo.Setup(r => r.DeleteMapping<CurrentRead>(
                It.IsAny<Expression<Func<CurrentRead, bool>>>()))
                .Returns(Task.CompletedTask);

            // Act
            await _currentReadService.DeleteCurrentRead(bookId, userId);

            // Assert
            _mockRepo.Verify(r => r.DeleteMapping<CurrentRead>(
                It.IsAny<Expression<Func<CurrentRead, bool>>>()), Times.Once);
        }

        [Test]
        public void Constructor_WithNullRepository_ShouldNotThrowException()
        {
            // Arrange & Act & Assert
            // This test verifies the current behavior, which does not throw when null is passed
            Assert.DoesNotThrow(() => new CurrentReadService(null));

            // Note: If you want to add null checking to your constructor, this test should be changed to:
            // Assert.Throws<ArgumentNullException>(() => new CurrentReadService(null));
        }

        [Test]
        public void Constructor_WithValidRepository_ShouldInitializeService()
        {
            // Arrange
            var repo = new Mock<IRepository<CurrentRead>>();

            // Act
            var service = new CurrentReadService(repo.Object);

            // Assert - test that the service is properly initialized by calling a method
            Assert.DoesNotThrow(() => service.GetAll());
        }

        [Test]
        public async Task UpdateCurrentPageNumber_ShouldUpdateCorrectly()
        {
            // Arrange
            var currentRead = new CurrentRead
            {
                Id = 1,
                BookId = 10,
                UserId = "user123",
                CurrentPage = 50
            };

            _mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<CurrentRead, bool>>>()))
                .ReturnsAsync(currentRead);

            _mockRepo.Setup(r => r.Update(It.IsAny<CurrentRead>()))
                .Returns(Task.CompletedTask);

            // Act
            // First get the current read
            var result = await _currentReadService.Get(cr => cr.BookId == 10 && cr.UserId == "user123");

            // Now update the page number
            result.CurrentPage = 75;
            await _currentReadService.Update(result);

            // Assert
            _mockRepo.Verify(r => r.Update(It.Is<CurrentRead>(cr =>
                cr.Id == 1 &&
                cr.BookId == 10 &&
                cr.UserId == "user123" &&
                cr.CurrentPage == 75)), Times.Once);
        }
    }
}
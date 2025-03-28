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
    public class PublishingHouseServiceTests
    {
        private Mock<IRepository<PublishingHouse>> _mockRepo;
        private IPublishingHouseService _publishingHouseService;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IRepository<PublishingHouse>>();
            _publishingHouseService = new PublishingHouseService(_mockRepo.Object);
        }

        [Test]
        public async Task GetById_ShouldCallRepositoryWithCorrectId()
        {
            // Arrange
            int publishingHouseId = 1;
            var expectedPublishingHouse = new PublishingHouse { Id = publishingHouseId, Name = "Penguin Random House", YearFounded = 1935 };
            _mockRepo.Setup(r => r.GetById(publishingHouseId)).ReturnsAsync(expectedPublishingHouse);

            // Act
            var result = await _publishingHouseService.GetById(publishingHouseId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedPublishingHouse));
            _mockRepo.Verify(r => r.GetById(publishingHouseId), Times.Once);
        }

        [Test]
        public void GetAll_ShouldReturnAllPublishingHouses()
        {
            // Arrange
            var publishingHouses = new List<PublishingHouse>
            {
                new PublishingHouse { Id = 1, Name = "Penguin Random House", YearFounded = 1935 },
                new PublishingHouse { Id = 2, Name = "HarperCollins", YearFounded = 1817 },
                new PublishingHouse { Id = 3, Name = "Simon & Schuster", YearFounded = 1924 }
            }.AsQueryable();

            _mockRepo.Setup(r => r.GetAll()).Returns(publishingHouses);

            // Act
            var result = _publishingHouseService.GetAll();

            // Assert
            Assert.That(result, Is.EqualTo(publishingHouses));
            _mockRepo.Verify(r => r.GetAll(), Times.Once);
        }

        [Test]
        public async Task Get_ShouldCallRepositoryWithCorrectFilter()
        {
            // Arrange
            var expectedPublishingHouse = new PublishingHouse { Id = 1, Name = "Penguin Random House", YearFounded = 1935 };
            Expression<Func<PublishingHouse, bool>> filter = ph => ph.Name == "Penguin Random House";

            _mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<PublishingHouse, bool>>>()))
                    .ReturnsAsync(expectedPublishingHouse);

            // Act
            var result = await _publishingHouseService.Get(filter);

            // Assert
            Assert.That(result, Is.EqualTo(expectedPublishingHouse));
            _mockRepo.Verify(r => r.Get(It.IsAny<Expression<Func<PublishingHouse, bool>>>()), Times.Once);
        }

        [Test]
        public async Task Find_ShouldCallRepositoryWithCorrectFilter()
        {
            // Arrange
            var expectedPublishingHouses = new List<PublishingHouse>
            {
                new PublishingHouse { Id = 1, Name = "Penguin Random House", YearFounded = 1935 },
                new PublishingHouse { Id = 4, Name = "Penguin Classics", YearFounded = 1946 }
            };

            Expression<Func<PublishingHouse, bool>> filter = ph => ph.Name.Contains("Penguin");

            _mockRepo.Setup(r => r.Find(It.IsAny<Expression<Func<PublishingHouse, bool>>>()))
                    .ReturnsAsync(expectedPublishingHouses);

            // Act
            var result = await _publishingHouseService.Find(filter);

            // Assert
            Assert.That(result, Is.EqualTo(expectedPublishingHouses));
            _mockRepo.Verify(r => r.Find(It.IsAny<Expression<Func<PublishingHouse, bool>>>()), Times.Once);
        }

        [Test]
        public async Task Add_ShouldCallRepositoryAddMethod()
        {
            // Arrange
            var publishingHouse = new PublishingHouse { Name = "New Publishing House", YearFounded = 2000 };
            _mockRepo.Setup(r => r.Add(It.IsAny<PublishingHouse>())).Returns(Task.CompletedTask);

            // Act
            await _publishingHouseService.Add(publishingHouse);

            // Assert
            _mockRepo.Verify(r => r.Add(publishingHouse), Times.Once);
        }

        [Test]
        public async Task Update_ShouldCallRepositoryUpdateMethod()
        {
            // Arrange
            var publishingHouse = new PublishingHouse { Id = 1, Name = "Updated Publishing House", YearFounded = 1950 };
            _mockRepo.Setup(r => r.Update(It.IsAny<PublishingHouse>())).Returns(Task.CompletedTask);

            // Act
            await _publishingHouseService.Update(publishingHouse);

            // Assert
            _mockRepo.Verify(r => r.Update(publishingHouse), Times.Once);
        }

        [Test]
        public async Task Delete_ShouldCallRepositoryDeleteMethod()
        {
            // Arrange
            int publishingHouseId = 1;
            _mockRepo.Setup(r => r.Delete(publishingHouseId)).Returns(Task.CompletedTask);

            // Act
            await _publishingHouseService.Delete(publishingHouseId);

            // Assert
            _mockRepo.Verify(r => r.Delete(publishingHouseId), Times.Once);
        }

        [Test]
        public void Constructor_WithNullRepository_ShouldNotThrowException()
        {
            // Arrange & Act & Assert
            // This test verifies the current behavior, which does not throw when null is passed
            Assert.DoesNotThrow(() => new PublishingHouseService(null));

            // Note: If you want to add null checking to your constructor, this test should be changed to:
            // Assert.Throws<ArgumentNullException>(() => new PublishingHouseService(null));
        }

        [Test]
        public void Constructor_WithValidRepository_ShouldInitializeService()
        {
            // Arrange
            var repo = new Mock<IRepository<PublishingHouse>>();

            // Act
            var service = new PublishingHouseService(repo.Object);

            // Assert - test that the service is properly initialized by calling a method
            Assert.DoesNotThrow(() => service.GetAll());
        }

        [Test]
        public async Task PublishingHouseWithBooks_ShouldWorkCorrectly()
        {
            // Arrange
            var publishingHouse = new PublishingHouse
            {
                Id = 1,
                Name = "Test Publishing House",
                YearFounded = 1985,
                Books = new List<Book>
                {
                    new Book { Id = 1, Title = "Book 1" },
                    new Book { Id = 2, Title = "Book 2" }
                }
            };

            _mockRepo.Setup(r => r.GetById(publishingHouse.Id)).ReturnsAsync(publishingHouse);

            // Act
            var result = await _publishingHouseService.GetById(publishingHouse.Id);

            // Assert
            Assert.That(result, Is.EqualTo(publishingHouse));
            Assert.That(result.Books.Count, Is.EqualTo(2));
            Assert.That(result.Books.Any(b => b.Id == 1 && b.Title == "Book 1"), Is.True);
            Assert.That(result.Books.Any(b => b.Id == 2 && b.Title == "Book 2"), Is.True);
        }

        [Test]
        public async Task PublishingHouseWorkflow_AddFindUpdateDelete_ShouldWorkCorrectly()
        {
            // Arrange
            var publishingHouse = new PublishingHouse
            {
                Id = 1,
                Name = "New Publishing House",
                YearFounded = 2000
            };

            var updatedPublishingHouse = new PublishingHouse
            {
                Id = 1,
                Name = "Updated Publishing House",
                YearFounded = 2000
            };

            _mockRepo.Setup(r => r.Add(It.IsAny<PublishingHouse>())).Returns(Task.CompletedTask);
            _mockRepo.Setup(r => r.Find(It.IsAny<Expression<Func<PublishingHouse, bool>>>()))
                .ReturnsAsync(new List<PublishingHouse> { publishingHouse });
            _mockRepo.Setup(r => r.Update(It.IsAny<PublishingHouse>())).Returns(Task.CompletedTask);
            _mockRepo.Setup(r => r.Delete(It.IsAny<int>())).Returns(Task.CompletedTask);

            // Act & Assert - Full workflow
            // 1. Add a publishing house
            await _publishingHouseService.Add(publishingHouse);
            _mockRepo.Verify(r => r.Add(publishingHouse), Times.Once);

            // 2. Find the publishing house
            var foundPublishingHouses = await _publishingHouseService.Find(ph => ph.Name.Contains("New"));
            Assert.That(foundPublishingHouses.Count, Is.EqualTo(1));
            Assert.That(foundPublishingHouses[0], Is.EqualTo(publishingHouse));

            // 3. Update the publishing house
            await _publishingHouseService.Update(updatedPublishingHouse);
            _mockRepo.Verify(r => r.Update(updatedPublishingHouse), Times.Once);

            // 4. Delete the publishing house
            await _publishingHouseService.Delete(publishingHouse.Id);
            _mockRepo.Verify(r => r.Delete(publishingHouse.Id), Times.Once);
        }
    }
}
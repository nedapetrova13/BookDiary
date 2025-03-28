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
            int currentReadId = 1;
            var expectedCurrentRead = new CurrentRead { Id = currentReadId, BookId = 10, UserId = "user123", CurrentPage = 50 };
            _mockRepo.Setup(r => r.GetById(currentReadId)).ReturnsAsync(expectedCurrentRead);

            var result = await _currentReadService.GetById(currentReadId);

            Assert.That(result, Is.EqualTo(expectedCurrentRead));
        }

        [Test]
        public void GetAll_ShouldReturnAllCurrentReads()
        {
            var currentReads = new List<CurrentRead>
            {
                new CurrentRead { Id = 1, BookId = 10, UserId = "user123", CurrentPage = 50 },
                new CurrentRead { Id = 2, BookId = 20, UserId = "user123", CurrentPage = 100 },
                new CurrentRead { Id = 3, BookId = 30, UserId = "user456", CurrentPage = 75 }
            }.AsQueryable();

            _mockRepo.Setup(r => r.GetAll()).Returns(currentReads);

            var result = _currentReadService.GetAll();

            Assert.That(result, Is.EqualTo(currentReads));
        }

        [Test]
        public async Task Get_ShouldCallRepositoryWithCorrectFilter()
        {
            var expectedCurrentRead = new CurrentRead { Id = 1, BookId = 10, UserId = "user123", CurrentPage = 50 };
            Expression<Func<CurrentRead, bool>> filter = cr => cr.BookId == 10 && cr.UserId == "user123";

            _mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<CurrentRead, bool>>>()))
                    .ReturnsAsync(expectedCurrentRead);

            var result = await _currentReadService.Get(filter);

            Assert.That(result, Is.EqualTo(expectedCurrentRead));
        }

        [Test]
        public async Task Find_ShouldCallRepositoryWithCorrectFilter()
        {
            var expectedCurrentReads = new List<CurrentRead>
            {
                new CurrentRead { Id = 1, BookId = 10, UserId = "user123", CurrentPage = 50 },
                new CurrentRead { Id = 2, BookId = 20, UserId = "user123", CurrentPage = 100 }
            };

            Expression<Func<CurrentRead, bool>> filter = cr => cr.UserId == "user123";

            _mockRepo.Setup(r => r.Find(It.IsAny<Expression<Func<CurrentRead, bool>>>()))
                    .ReturnsAsync(expectedCurrentReads);

            var result = await _currentReadService.Find(filter);

            Assert.That(result, Is.EqualTo(expectedCurrentReads));
        }

        [Test]
        public async Task Add_ShouldCallRepositoryAddMethod()
        {
            var currentRead = new CurrentRead { BookId = 10, UserId = "user123", CurrentPage = 50 };
            _mockRepo.Setup(r => r.Add(It.IsAny<CurrentRead>())).Returns(Task.CompletedTask);

            await _currentReadService.Add(currentRead);

        }

        [Test]
        public async Task Update_ShouldCallRepositoryUpdateMethod()
        {
            var currentRead = new CurrentRead { Id = 1, BookId = 10, UserId = "user123", CurrentPage = 75 };
            _mockRepo.Setup(r => r.Update(It.IsAny<CurrentRead>())).Returns(Task.CompletedTask);

            await _currentReadService.Update(currentRead);
        }

        [Test]
        public async Task Delete_ShouldCallRepositoryDeleteMethod()
        {
            int currentReadId = 1;
            _mockRepo.Setup(r => r.Delete(currentReadId)).Returns(Task.CompletedTask);

            await _currentReadService.Delete(currentReadId);
        }

        [Test]
        public async Task DeleteCurrentRead_ShouldCallRepositoryDeleteMappingMethod()
        {
            int bookId = 10;
            string userId = "user123";

            _mockRepo.Setup(r => r.DeleteMapping<CurrentRead>(
                It.IsAny<Expression<Func<CurrentRead, bool>>>()))
                .Returns(Task.CompletedTask);

            await _currentReadService.DeleteCurrentRead(bookId, userId);

        }

        [Test]
        public void Constructor_WithNullRepository_ShouldNotThrowException()
        {
            Assert.DoesNotThrow(() => new CurrentReadService(null));

        }

        [Test]
        public void Constructor_WithValidRepository_ShouldInitializeService()
        {
            var repo = new Mock<IRepository<CurrentRead>>();

            var service = new CurrentReadService(repo.Object);

            Assert.DoesNotThrow(() => service.GetAll());
        }

        [Test]
        public async Task UpdateCurrentPageNumber_ShouldUpdateCorrectly()
        {
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

            var result = await _currentReadService.Get(cr => cr.BookId == 10 && cr.UserId == "user123");

            result.CurrentPage = 75;
            await _currentReadService.Update(result);

            
        }
    }
}
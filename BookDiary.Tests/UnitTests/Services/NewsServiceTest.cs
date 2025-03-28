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
    public class NewsServiceTests
    {
        private Mock<IRepository<News>> _mockRepo;
        private INewsService _newsService;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IRepository<News>>();
            _newsService = new NewsService(_mockRepo.Object);
        }

        [Test]
        public async Task GetById_ShouldCallRepositoryWithCorrectId()
        {
            // Arrange
            int newsId = 1;
            var expectedNews = new News { Id = newsId, Title = "Test News", Content = "Test Content" };
            _mockRepo.Setup(r => r.GetById(newsId)).ReturnsAsync(expectedNews);

            // Act
            var result = await _newsService.GetById(newsId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedNews));
            _mockRepo.Verify(r => r.GetById(newsId), Times.Once);
        }

        [Test]
        public void GetAll_ShouldReturnAllNews()
        {
            // Arrange
            var newsList = new List<News>
            {
                new News { Id = 1, Title = "News 1", Content = "Content 1" },
                new News { Id = 2, Title = "News 2", Content = "Content 2" },
                new News { Id = 3, Title = "News 3", Content = "Content 3" }
            }.AsQueryable();

            _mockRepo.Setup(r => r.GetAll()).Returns(newsList);

            // Act
            var result = _newsService.GetAll();

            // Assert
            Assert.That(result, Is.EqualTo(newsList));
            _mockRepo.Verify(r => r.GetAll(), Times.Once);
        }

        [Test]
        public async Task Get_ShouldCallRepositoryWithCorrectFilter()
        {
            // Arrange
            var expectedNews = new News { Id = 1, Title = "Test News", Content = "Test Content" };
            Expression<Func<News, bool>> filter = n => n.Title == "Test News";

            _mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<News, bool>>>()))
                    .ReturnsAsync(expectedNews);

            // Act
            var result = await _newsService.Get(filter);

            // Assert
            Assert.That(result, Is.EqualTo(expectedNews));
            _mockRepo.Verify(r => r.Get(It.IsAny<Expression<Func<News, bool>>>()), Times.Once);
        }

        [Test]
        public async Task Find_ShouldCallRepositoryWithCorrectFilter()
        {
            // Arrange
            var expectedNews = new List<News>
            {
                new News { Id = 1, Title = "Test News 1", Content = "Test Content 1" },
                new News { Id = 2, Title = "Test News 2", Content = "Test Content 2" }
            };

            Expression<Func<News, bool>> filter = n => n.Title.Contains("Test");

            _mockRepo.Setup(r => r.Find(It.IsAny<Expression<Func<News, bool>>>()))
                    .ReturnsAsync(expectedNews);

            // Act
            var result = await _newsService.Find(filter);

            // Assert
            Assert.That(result, Is.EqualTo(expectedNews));
            _mockRepo.Verify(r => r.Find(It.IsAny<Expression<Func<News, bool>>>()), Times.Once);
        }

        [Test]
        public async Task Add_ShouldCallRepositoryAddMethod()
        {
            // Arrange
            var news = new News { Title = "New News", Content = "New Content" };
            _mockRepo.Setup(r => r.Add(It.IsAny<News>())).Returns(Task.CompletedTask);

            // Act
            await _newsService.Add(news);

            // Assert
            _mockRepo.Verify(r => r.Add(news), Times.Once);
        }

        [Test]
        public async Task Update_ShouldCallRepositoryUpdateMethod()
        {
            // Arrange
            var news = new News { Id = 1, Title = "Updated News", Content = "Updated Content" };
            _mockRepo.Setup(r => r.Update(It.IsAny<News>())).Returns(Task.CompletedTask);

            // Act
            await _newsService.Update(news);

            // Assert
            _mockRepo.Verify(r => r.Update(news), Times.Once);
        }

        [Test]
        public async Task Delete_ShouldCallRepositoryDeleteMethod()
        {
            // Arrange
            int newsId = 1;
            _mockRepo.Setup(r => r.Delete(newsId)).Returns(Task.CompletedTask);

            // Act
            await _newsService.Delete(newsId);

            // Assert
            _mockRepo.Verify(r => r.Delete(newsId), Times.Once);
        }

        [Test]
        public async Task GetTop5Services_ShouldReturnTop3NewsByCreationDate()
        {
            // Arrange
            var newsList = new List<News>
            {
                new News { Id = 1, Title = "News 1", Content = "Content 1", Created = DateTime.Now.AddDays(-5) },
                new News { Id = 2, Title = "News 2", Content = "Content 2", Created = DateTime.Now.AddDays(-1) },
                new News { Id = 3, Title = "News 3", Content = "Content 3", Created = DateTime.Now.AddDays(-10) },
                new News { Id = 4, Title = "News 4", Content = "Content 4", Created = DateTime.Now },
                new News { Id = 5, Title = "News 5", Content = "Content 5", Created = DateTime.Now.AddDays(-3) }
            }.AsQueryable();

            _mockRepo.Setup(r => r.GetAll()).Returns(newsList);

            // Act
            var result = await _newsService.GetTop5Services();
            var resultList = result.ToList();

            // Assert
            Assert.That(resultList.Count, Is.EqualTo(3), "Should return exactly 3 news items");

            // Verify they're in correct descending order by Created date
            Assert.That(resultList[0].Id, Is.EqualTo(4), "First news should be the most recent");
            Assert.That(resultList[1].Id, Is.EqualTo(2), "Second news should be the second most recent");
            Assert.That(resultList[2].Id, Is.EqualTo(5), "Third news should be the third most recent");

            _mockRepo.Verify(r => r.GetAll(), Times.Once);
        }

        [Test]
        public void Constructor_WithNullRepository_ShouldNotThrowException()
        {
            // Arrange & Act & Assert
            // This test verifies the current behavior, which does not throw when null is passed
            Assert.DoesNotThrow(() => new NewsService(null));

            // Note: If you want to add null checking to your constructor, this test should be changed to:
            // Assert.Throws<ArgumentNullException>(() => new NewsService(null));
        }

        [Test]
        public void Constructor_WithValidRepository_ShouldInitializeService()
        {
            // Arrange
            var repo = new Mock<IRepository<News>>();

            // Act
            var service = new NewsService(repo.Object);

            // Assert - test that the service is properly initialized by calling a method
            Assert.DoesNotThrow(() => service.GetAll());
        }

        [Test]
        public async Task NewsWorkflow_AddGetUpdateDelete_ShouldWorkCorrectly()
        {
            // Arrange
            var news = new News
            {
                Id = 1,
                Title = "Breaking News",
                Content = "Something important happened",
                Created = DateTime.Now
            };

            var updatedNews = new News
            {
                Id = 1,
                Title = "Breaking News Update",
                Content = "New developments have occurred",
                Created = DateTime.Now
            };

            _mockRepo.Setup(r => r.Add(It.IsAny<News>())).Returns(Task.CompletedTask);
            _mockRepo.Setup(r => r.GetById(news.Id)).ReturnsAsync(news);
            _mockRepo.Setup(r => r.Update(It.IsAny<News>())).Returns(Task.CompletedTask);
            _mockRepo.Setup(r => r.Delete(It.IsAny<int>())).Returns(Task.CompletedTask);

            // Act & Assert - Full workflow
            // 1. Add news
            await _newsService.Add(news);
            _mockRepo.Verify(r => r.Add(news), Times.Once);

            // 2. Get the news by id
            var foundNews = await _newsService.GetById(news.Id);
            Assert.That(foundNews, Is.EqualTo(news));

            // 3. Update the news
            await _newsService.Update(updatedNews);
            _mockRepo.Verify(r => r.Update(updatedNews), Times.Once);

            // 4. Delete the news
            await _newsService.Delete(news.Id);
            _mockRepo.Verify(r => r.Delete(news.Id), Times.Once);
        }
    }
}
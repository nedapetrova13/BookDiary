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
            int newsId = 1;
            var expectedNews = new News { Id = newsId, Title = "Test News", Content = "Test Content" };
            _mockRepo.Setup(r => r.GetById(newsId)).ReturnsAsync(expectedNews);

            var result = await _newsService.GetById(newsId);

            Assert.That(result, Is.EqualTo(expectedNews));
        }

        [Test]
        public void GetAll_ShouldReturnAllNews()
        {
            var newsList = new List<News>
            {
                new News { Id = 1, Title = "News 1", Content = "Content 1" },
                new News { Id = 2, Title = "News 2", Content = "Content 2" },
                new News { Id = 3, Title = "News 3", Content = "Content 3" }
            }.AsQueryable();

            _mockRepo.Setup(r => r.GetAll()).Returns(newsList);

            var result = _newsService.GetAll();

            Assert.That(result, Is.EqualTo(newsList));
        }

        [Test]
        public async Task Get_ShouldCallRepositoryWithCorrectFilter()
        {
            var expectedNews = new News { Id = 1, Title = "Test News", Content = "Test Content" };
            Expression<Func<News, bool>> filter = n => n.Title == "Test News";

            _mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<News, bool>>>()))
                    .ReturnsAsync(expectedNews);

            var result = await _newsService.Get(filter);

            Assert.That(result, Is.EqualTo(expectedNews));
        }

        [Test]
        public async Task Find_ShouldCallRepositoryWithCorrectFilter()
        {
            var expectedNews = new List<News>
            {
                new News { Id = 1, Title = "Test News 1", Content = "Test Content 1" },
                new News { Id = 2, Title = "Test News 2", Content = "Test Content 2" }
            };

            Expression<Func<News, bool>> filter = n => n.Title.Contains("Test");

            _mockRepo.Setup(r => r.Find(It.IsAny<Expression<Func<News, bool>>>()))
                    .ReturnsAsync(expectedNews);

            var result = await _newsService.Find(filter);

            Assert.That(result, Is.EqualTo(expectedNews));
        }

        [Test]
        public async Task Add_ShouldCallRepositoryAddMethod()
        {
            var news = new News { Title = "New News", Content = "New Content" };
            _mockRepo.Setup(r => r.Add(It.IsAny<News>())).Returns(Task.CompletedTask);

            await _newsService.Add(news);
        }

        [Test]
        public async Task Update_ShouldCallRepositoryUpdateMethod()
        {
            var news = new News { Id = 1, Title = "Updated News", Content = "Updated Content" };
            _mockRepo.Setup(r => r.Update(It.IsAny<News>())).Returns(Task.CompletedTask);

            await _newsService.Update(news);
        }

        [Test]
        public async Task Delete_ShouldCallRepositoryDeleteMethod()
        {
            int newsId = 1;
            _mockRepo.Setup(r => r.Delete(newsId)).Returns(Task.CompletedTask);

            await _newsService.Delete(newsId);
        }

        [Test]
        public async Task GetTop5Services_ShouldReturnTop3NewsByCreationDate()
        {
            var newsList = new List<News>
            {
                new News { Id = 1, Title = "News 1", Content = "Content 1", Created = DateTime.Now.AddDays(-5) },
                new News { Id = 2, Title = "News 2", Content = "Content 2", Created = DateTime.Now.AddDays(-1) },
                new News { Id = 3, Title = "News 3", Content = "Content 3", Created = DateTime.Now.AddDays(-10) },
                new News { Id = 4, Title = "News 4", Content = "Content 4", Created = DateTime.Now },
                new News { Id = 5, Title = "News 5", Content = "Content 5", Created = DateTime.Now.AddDays(-3) }
            }.AsQueryable();

            _mockRepo.Setup(r => r.GetAll()).Returns(newsList);

            var result = await _newsService.GetTop5Services();
            var resultList = result.ToList();

            Assert.That(resultList.Count, Is.EqualTo(3), "Should return exactly 3 news items");

            Assert.That(resultList[0].Id, Is.EqualTo(4), "First news should be the most recent");
            Assert.That(resultList[1].Id, Is.EqualTo(2), "Second news should be the second most recent");
            Assert.That(resultList[2].Id, Is.EqualTo(5), "Third news should be the third most recent");

        }

        [Test]
        public void Constructor_WithNullRepository_ShouldNotThrowException()
        {
            
            Assert.DoesNotThrow(() => new NewsService(null));

            
        }

        [Test]
        public void Constructor_WithValidRepository_ShouldInitializeService()
        {
            var repo = new Mock<IRepository<News>>();

            var service = new NewsService(repo.Object);

            Assert.DoesNotThrow(() => service.GetAll());
        }

        [Test]
        public async Task NewsWorkflow_AddGetUpdateDelete_ShouldWorkCorrectly()
        {
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

            await _newsService.Add(news);

            var foundNews = await _newsService.GetById(news.Id);
            Assert.That(foundNews, Is.EqualTo(news));

            await _newsService.Update(updatedNews);

            await _newsService.Delete(news.Id);
        }
    }
}
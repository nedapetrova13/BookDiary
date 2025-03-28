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
    public class SeriesServiceTests
    {
        private Mock<IRepository<Series>> _mockRepo;
        private ISeriesService _seriesService;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IRepository<Series>>();
            _seriesService = new SeriesService(_mockRepo.Object);
        }

        [Test]
        public async Task GetById_ShouldCallRepositoryWithCorrectId()
        {
            // Arrange
            int seriesId = 1;
            var expectedSeries = new Series { Id = seriesId, Title = "Harry Potter" };
            _mockRepo.Setup(r => r.GetById(seriesId)).ReturnsAsync(expectedSeries);

            // Act
            var result = await _seriesService.GetById(seriesId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedSeries));
            _mockRepo.Verify(r => r.GetById(seriesId), Times.Once);
        }

        [Test]
        public void GetAll_ShouldReturnAllSeries()
        {
            // Arrange
            var seriesList = new List<Series>
            {
                new Series { Id = 1, Title = "Harry Potter" },
                new Series { Id = 2, Title = "Lord of the Rings" },
                new Series { Id = 3, Title = "The Hunger Games" }
            }.AsQueryable();

            _mockRepo.Setup(r => r.GetAll()).Returns(seriesList);

            // Act
            var result = _seriesService.GetAll();

            // Assert
            Assert.That(result, Is.EqualTo(seriesList));
            _mockRepo.Verify(r => r.GetAll(), Times.Once);
        }

        [Test]
        public async Task Get_ShouldCallRepositoryWithCorrectFilter()
        {
            // Arrange
            var expectedSeries = new Series { Id = 1, Title = "Harry Potter" };
            Expression<Func<Series, bool>> filter = s => s.Title == "Harry Potter";

            _mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<Series, bool>>>()))
                    .ReturnsAsync(expectedSeries);

            // Act
            var result = await _seriesService.Get(filter);

            // Assert
            Assert.That(result, Is.EqualTo(expectedSeries));
            _mockRepo.Verify(r => r.Get(It.IsAny<Expression<Func<Series, bool>>>()), Times.Once);
        }

        [Test]
        public async Task Find_ShouldCallRepositoryWithCorrectFilter()
        {
            // Arrange
            var expectedSeries = new List<Series>
            {
                new Series { Id = 1, Title = "Harry Potter" },
                new Series { Id = 4, Title = "Harry Dresden Files" }
            };

            Expression<Func<Series, bool>> filter = s => s.Title.Contains("Harry");

            _mockRepo.Setup(r => r.Find(It.IsAny<Expression<Func<Series, bool>>>()))
                    .ReturnsAsync(expectedSeries);

            // Act
            var result = await _seriesService.Find(filter);

            // Assert
            Assert.That(result, Is.EqualTo(expectedSeries));
            _mockRepo.Verify(r => r.Find(It.IsAny<Expression<Func<Series, bool>>>()), Times.Once);
        }

        [Test]
        public async Task Add_ShouldCallRepositoryAddMethod()
        {
            // Arrange
            var series = new Series { Title = "New Series" };
            _mockRepo.Setup(r => r.Add(It.IsAny<Series>())).Returns(Task.CompletedTask);

            // Act
            await _seriesService.Add(series);

            // Assert
            _mockRepo.Verify(r => r.Add(series), Times.Once);
        }

        [Test]
        public async Task Update_ShouldCallRepositoryUpdateMethod()
        {
            // Arrange
            var series = new Series { Id = 1, Title = "Updated Series" };
            _mockRepo.Setup(r => r.Update(It.IsAny<Series>())).Returns(Task.CompletedTask);

            // Act
            await _seriesService.Update(series);

            // Assert
            _mockRepo.Verify(r => r.Update(series), Times.Once);
        }

        [Test]
        public async Task Delete_ShouldCallRepositoryDeleteMethod()
        {
            // Arrange
            int seriesId = 1;
            _mockRepo.Setup(r => r.Delete(seriesId)).Returns(Task.CompletedTask);

            // Act
            await _seriesService.Delete(seriesId);

            // Assert
            _mockRepo.Verify(r => r.Delete(seriesId), Times.Once);
        }

        [Test]
        public void Constructor_WithNullRepository_ShouldNotThrowException()
        {
            // Arrange & Act & Assert
            // This test verifies the current behavior, which does not throw when null is passed
            Assert.DoesNotThrow(() => new SeriesService(null));

            // Note: If you want to add null checking to your constructor, this test should be changed to:
            // Assert.Throws<ArgumentNullException>(() => new SeriesService(null));
        }

        [Test]
        public void Constructor_WithValidRepository_ShouldInitializeService()
        {
            // Arrange
            var repo = new Mock<IRepository<Series>>();

            // Act
            var service = new SeriesService(repo.Object);

            // Assert - test that the service is properly initialized by calling a method
            Assert.DoesNotThrow(() => service.GetAll());
        }

        [Test]
        public async Task SeriesWithBooks_ShouldWorkCorrectly()
        {
            // Arrange
            var series = new Series
            {
                Id = 1,
                Title = "Test Series",
                Books = new List<Book>
                {
                    new Book { Id = 1, Title = "Book 1" },
                    new Book { Id = 2, Title = "Book 2" },
                    new Book { Id = 3, Title = "Book 3" }
                }
            };

            _mockRepo.Setup(r => r.GetById(series.Id)).ReturnsAsync(series);

            // Act
            var result = await _seriesService.GetById(series.Id);

            // Assert
            Assert.That(result, Is.EqualTo(series));
            Assert.That(result.Books.Count, Is.EqualTo(3));
            Assert.That(result.Books.Any(b => b.Id == 1 && b.Title == "Book 1"), Is.True);
            Assert.That(result.Books.Any(b => b.Id == 2 && b.Title == "Book 2"), Is.True);
            Assert.That(result.Books.Any(b => b.Id == 3 && b.Title == "Book 3"), Is.True);
        }

        [Test]
        public async Task SeriesWorkflow_AddFindUpdateDelete_ShouldWorkCorrectly()
        {
            // Arrange
            var series = new Series
            {
                Id = 1,
                Title = "Fantasy Series"
            };

            var updatedSeries = new Series
            {
                Id = 1,
                Title = "Epic Fantasy Series"
            };

            _mockRepo.Setup(r => r.Add(It.IsAny<Series>())).Returns(Task.CompletedTask);
            _mockRepo.Setup(r => r.Find(It.IsAny<Expression<Func<Series, bool>>>()))
                .ReturnsAsync(new List<Series> { series });
            _mockRepo.Setup(r => r.Update(It.IsAny<Series>())).Returns(Task.CompletedTask);
            _mockRepo.Setup(r => r.Delete(It.IsAny<int>())).Returns(Task.CompletedTask);

            // Act & Assert - Full workflow
            // 1. Add a series
            await _seriesService.Add(series);
            _mockRepo.Verify(r => r.Add(series), Times.Once);

            // 2. Find the series
            var foundSeries = await _seriesService.Find(s => s.Title.Contains("Fantasy"));
            Assert.That(foundSeries.Count, Is.EqualTo(1));
            Assert.That(foundSeries[0], Is.EqualTo(series));

            // 3. Update the series
            await _seriesService.Update(updatedSeries);
            _mockRepo.Verify(r => r.Update(updatedSeries), Times.Once);

            // 4. Delete the series
            await _seriesService.Delete(series.Id);
            _mockRepo.Verify(r => r.Delete(series.Id), Times.Once);
        }
    }
}
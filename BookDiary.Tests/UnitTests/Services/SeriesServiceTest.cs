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
            int seriesId = 1;
            var expectedSeries = new Series { Id = seriesId, Title = "Harry Potter" };
            _mockRepo.Setup(r => r.GetById(seriesId)).ReturnsAsync(expectedSeries);

            var result = await _seriesService.GetById(seriesId);

            Assert.That(result, Is.EqualTo(expectedSeries));
        }

        [Test]
        public void GetAll_ShouldReturnAllSeries()
        {
             var seriesList = new List<Series>
            {
                new Series { Id = 1, Title = "Harry Potter" },
                new Series { Id = 2, Title = "Lord of the Rings" },
                new Series { Id = 3, Title = "The Hunger Games" }
            }.AsQueryable();

            _mockRepo.Setup(r => r.GetAll()).Returns(seriesList);

            var result = _seriesService.GetAll();

            Assert.That(result, Is.EqualTo(seriesList));
        }

        [Test]
        public async Task Get_ShouldCallRepositoryWithCorrectFilter()
        {
            var expectedSeries = new Series { Id = 1, Title = "Harry Potter" };
            Expression<Func<Series, bool>> filter = s => s.Title == "Harry Potter";

            _mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<Series, bool>>>()))
                    .ReturnsAsync(expectedSeries);

            var result = await _seriesService.Get(filter);

            Assert.That(result, Is.EqualTo(expectedSeries));
        }

        [Test]
        public async Task Find_ShouldCallRepositoryWithCorrectFilter()
        {
            var expectedSeries = new List<Series>
            {
                new Series { Id = 1, Title = "Harry Potter" },
                new Series { Id = 4, Title = "Harry Dresden Files" }
            };

            Expression<Func<Series, bool>> filter = s => s.Title.Contains("Harry");

            _mockRepo.Setup(r => r.Find(It.IsAny<Expression<Func<Series, bool>>>()))
                    .ReturnsAsync(expectedSeries);

            var result = await _seriesService.Find(filter);

            Assert.That(result, Is.EqualTo(expectedSeries));
        }

        [Test]
        public async Task Add_ShouldCallRepositoryAddMethod()
        {
            var series = new Series { Title = "New Series" };
            _mockRepo.Setup(r => r.Add(It.IsAny<Series>())).Returns(Task.CompletedTask);

            await _seriesService.Add(series);
        }

        [Test]
        public async Task Update_ShouldCallRepositoryUpdateMethod()
        {
            var series = new Series { Id = 1, Title = "Updated Series" };
            _mockRepo.Setup(r => r.Update(It.IsAny<Series>())).Returns(Task.CompletedTask);

            await _seriesService.Update(series);
        }

        [Test]
        public async Task Delete_ShouldCallRepositoryDeleteMethod()
        {
            int seriesId = 1;
            _mockRepo.Setup(r => r.Delete(seriesId)).Returns(Task.CompletedTask);

            await _seriesService.Delete(seriesId);
        }

        [Test]
        public void Constructor_WithNullRepository_ShouldNotThrowException()
        {
            
            Assert.DoesNotThrow(() => new SeriesService(null));

            
        }

        [Test]
        public void Constructor_WithValidRepository_ShouldInitializeService()
        {
            var repo = new Mock<IRepository<Series>>();

            var service = new SeriesService(repo.Object);

            Assert.DoesNotThrow(() => service.GetAll());
        }

        [Test]
        public async Task SeriesWithBooks_ShouldWorkCorrectly()
        {
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

            var result = await _seriesService.GetById(series.Id);

            Assert.That(result, Is.EqualTo(series));
            Assert.That(result.Books.Count, Is.EqualTo(3));
            Assert.That(result.Books.Any(b => b.Id == 1 && b.Title == "Book 1"), Is.True);
            Assert.That(result.Books.Any(b => b.Id == 2 && b.Title == "Book 2"), Is.True);
            Assert.That(result.Books.Any(b => b.Id == 3 && b.Title == "Book 3"), Is.True);
        }

        [Test]
        public async Task SeriesWorkflow_AddFindUpdateDelete_ShouldWorkCorrectly()
        {
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

            await _seriesService.Add(series);

            var foundSeries = await _seriesService.Find(s => s.Title.Contains("Fantasy"));
            Assert.That(foundSeries.Count, Is.EqualTo(1));
            Assert.That(foundSeries[0], Is.EqualTo(series));

            await _seriesService.Update(updatedSeries);

            await _seriesService.Delete(series.Id);
=        }
    }
}
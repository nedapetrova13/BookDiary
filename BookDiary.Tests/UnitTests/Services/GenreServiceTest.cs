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
    public class GenreServiceTests
    {
        private Mock<IRepository<Genre>> _mockRepo;
        private IGenreService _genreService;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IRepository<Genre>>();
            _genreService = new GenreService(_mockRepo.Object);
        }

        [Test]
        public async Task GetById_ShouldCallRepositoryWithCorrectId()
        {
            int genreId = 1;
            var expectedGenre = new Genre { Id = genreId, Name = "Fantasy" };
            _mockRepo.Setup(r => r.GetById(genreId)).ReturnsAsync(expectedGenre);

            var result = await _genreService.GetById(genreId);

            Assert.That(result, Is.EqualTo(expectedGenre));
        }

        [Test]
        public void GetAll_ShouldReturnAllGenres()
        {
            var genres = new List<Genre>
            {
                new Genre { Id = 1, Name = "Fantasy" },
                new Genre { Id = 2, Name = "Science Fiction" },
                new Genre { Id = 3, Name = "Mystery" }
            }.AsQueryable();

            _mockRepo.Setup(r => r.GetAll()).Returns(genres);

            var result = _genreService.GetAll();

            Assert.That(result, Is.EqualTo(genres));
        }

        [Test]
        public async Task Get_ShouldCallRepositoryWithCorrectFilter()
        {
            var expectedGenre = new Genre { Id = 1, Name = "Fantasy" };
            Expression<Func<Genre, bool>> filter = g => g.Name == "Fantasy";

            _mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<Genre, bool>>>()))
                    .ReturnsAsync(expectedGenre);

            var result = await _genreService.Get(filter);

            Assert.That(result, Is.EqualTo(expectedGenre));
        }

        [Test]
        public async Task Find_ShouldCallRepositoryWithCorrectFilter()
        {
            var expectedGenres = new List<Genre>
            {
                new Genre { Id = 1, Name = "Fantasy" },
                new Genre { Id = 4, Name = "Dark Fantasy" }
            };

            Expression<Func<Genre, bool>> filter = g => g.Name.Contains("Fantasy");

            _mockRepo.Setup(r => r.Find(It.IsAny<Expression<Func<Genre, bool>>>()))
                    .ReturnsAsync(expectedGenres);

            var result = await _genreService.Find(filter);

            Assert.That(result, Is.EqualTo(expectedGenres));
        }

        [Test]
        public async Task Add_ShouldCallRepositoryAddMethod()
        {
            var genre = new Genre { Name = "New Genre" };
            _mockRepo.Setup(r => r.Add(It.IsAny<Genre>())).Returns(Task.CompletedTask);

            await _genreService.Add(genre);
        }

        [Test]
        public async Task Update_ShouldCallRepositoryUpdateMethod()
        {
            var genre = new Genre { Id = 1, Name = "Updated Genre" };
            _mockRepo.Setup(r => r.Update(It.IsAny<Genre>())).Returns(Task.CompletedTask);

            await _genreService.Update(genre);
        }

        [Test]
        public async Task Delete_ShouldCallRepositoryDeleteMethod()
        {
            int genreId = 1;
            _mockRepo.Setup(r => r.Delete(genreId)).Returns(Task.CompletedTask);

            await _genreService.Delete(genreId);
        }

        [Test]
        public void Constructor_WithNullRepository_ShouldNotThrowException()
        {
            Assert.DoesNotThrow(() => new GenreService(null));

        }

        [Test]
        public void Constructor_WithValidRepository_ShouldInitializeService()
        {
            var repo = new Mock<IRepository<Genre>>();

            var service = new GenreService(repo.Object);

            Assert.DoesNotThrow(() => service.GetAll());
        }

        [Test]
        public async Task GenreWorkflow_AddFindUpdateDelete_ShouldWorkCorrectly()
        {
            var genre = new Genre { Id = 1, Name = "Fantasy" };
            var updatedGenre = new Genre { Id = 1, Name = "Epic Fantasy" };

            _mockRepo.Setup(r => r.Add(It.IsAny<Genre>())).Returns(Task.CompletedTask);
            _mockRepo.Setup(r => r.Find(It.IsAny<Expression<Func<Genre, bool>>>()))
                .ReturnsAsync(new List<Genre> { genre });
            _mockRepo.Setup(r => r.Update(It.IsAny<Genre>())).Returns(Task.CompletedTask);
            _mockRepo.Setup(r => r.Delete(It.IsAny<int>())).Returns(Task.CompletedTask);

            await _genreService.Add(genre);

            var foundGenres = await _genreService.Find(g => g.Name.Contains("Fantasy"));
            Assert.That(foundGenres.Count, Is.EqualTo(1));
            Assert.That(foundGenres[0], Is.EqualTo(genre));

            await _genreService.Update(updatedGenre);

            await _genreService.Delete(genre.Id);
        }
    }
}
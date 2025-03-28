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
using BookDiary.Core.Validators;
using System.Reflection;

namespace BookDiary.Tests.UnitTests.Services
{
    [TestFixture]
    public class AuthorServiceTests
    {
        private Mock<IRepository<Author>> _mockRepo;
        private IAuthorService _authorService;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IRepository<Author>>();
            _authorService = new AuthorService(_mockRepo.Object);
        }

        [Test]
        public async Task GetById_ShouldCallRepositoryWithCorrectId()
        {
            // Arrange
            int authorId = 1;
            var expectedAuthor = new Author { Id = authorId, Name = "Test Author" };
            _mockRepo.Setup(r => r.GetById(authorId)).ReturnsAsync(expectedAuthor);

            // Act
            var result = await _authorService.GetById(authorId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedAuthor));
            _mockRepo.Verify(r => r.GetById(authorId), Times.Once);
        }

        [Test]
        public void GetAll_ShouldReturnAllAuthors()
        {
            // Arrange
            var authors = new List<Author>
            {
                new Author { Id = 1, Name = "Author 1" },
                new Author { Id = 2, Name = "Author 2" },
                new Author { Id = 3, Name = "Author 3" }
            }.AsQueryable();

            _mockRepo.Setup(r => r.GetAll()).Returns(authors);

            // Act
            var result = _authorService.GetAll();

            // Assert
            Assert.That(result, Is.EqualTo(authors));
            _mockRepo.Verify(r => r.GetAll(), Times.Once);
        }

        [Test]
        public async Task Get_ShouldCallRepositoryWithCorrectFilter()
        {
            // Arrange
            var expectedAuthor = new Author { Id = 1, Name = "Test Author" };
            Expression<Func<Author, bool>> filter = a => a.Id == 1;

            _mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<Author, bool>>>()))
                    .ReturnsAsync(expectedAuthor);

            // Act
            var result = await _authorService.Get(filter);

            // Assert
            Assert.That(result, Is.EqualTo(expectedAuthor));
            _mockRepo.Verify(r => r.Get(It.IsAny<Expression<Func<Author, bool>>>()), Times.Once);
        }

        [Test]
        public async Task Find_ShouldCallRepositoryWithCorrectFilter()
        {
            // Arrange
            var expectedAuthors = new List<Author>
            {
                new Author { Id = 1, Name = "Author 1" },
                new Author { Id = 2, Name = "Author 2" }
            };

            Expression<Func<Author, bool>> filter = a => a.Name.Contains("Author");

            _mockRepo.Setup(r => r.Find(It.IsAny<Expression<Func<Author, bool>>>()))
                    .ReturnsAsync(expectedAuthors);

            // Act
            var result = await _authorService.Find(filter);

            // Assert
            Assert.That(result, Is.EqualTo(expectedAuthors));
            _mockRepo.Verify(r => r.Find(It.IsAny<Expression<Func<Author, bool>>>()), Times.Once);
        }

        [Test]
        public async Task Add_ShouldCallRepositoryAddMethod()
        {
            // Arrange
            var author = new Author { Id = 1, Name = "New Author" };
            _mockRepo.Setup(r => r.Add(It.IsAny<Author>())).Returns(Task.CompletedTask);

            // Act
            await _authorService.Add(author);

            // Assert
            _mockRepo.Verify(r => r.Add(author), Times.Once);
        }

        [Test]
        public async Task Update_ShouldCallRepositoryUpdateMethod()
        {
            // Arrange
            var author = new Author { Id = 1, Name = "Updated Author" };
            _mockRepo.Setup(r => r.Update(It.IsAny<Author>())).Returns(Task.CompletedTask);

            // Act
            await _authorService.Update(author);

            // Assert
            _mockRepo.Verify(r => r.Update(author), Times.Once);
        }

        [Test]
        public async Task Delete_ShouldCallRepositoryDeleteMethod()
        {
            // Arrange
            int authorId = 1;
            _mockRepo.Setup(r => r.Delete(authorId)).Returns(Task.CompletedTask);

            // Act
            await _authorService.Delete(authorId);

            // Assert
            _mockRepo.Verify(r => r.Delete(authorId), Times.Once);
        }

        // Note: The following tests will need to be updated once you have a way to properly
        // test or mock the static AuthorValidator. We're commenting them out for now.
        /*
        [Test]
        public void ValidateAuthor_WithValidAuthor_ShouldReturnTrue()
        {
            // This requires a way to mock static methods, which is not straightforward in .NET
            // Consider refactoring AuthorValidator to be non-static or use a library like Fody
        }

        [Test]
        public void ValidateAuthor_WithInvalidInput_ShouldReturnFalse()
        {
            // This requires a way to mock static methods
        }

        [Test]
        public void ValidateAuthor_WithNonExistingAuthor_ShouldReturnFalse()
        {
            // This requires a way to mock static methods
        }
        */

        // Helper method to invoke private methods using reflection
        private object InvokePrivateMethod(object instance, string methodName, params object[] parameters)
        {
            Type type = instance.GetType();
            var method = type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            return method.Invoke(instance, parameters);
        }
    }
}
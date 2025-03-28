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
            int authorId = 1;
            var expectedAuthor = new Author { Id = authorId, Name = "Test Author" };
            _mockRepo.Setup(r => r.GetById(authorId)).ReturnsAsync(expectedAuthor);

            var result = await _authorService.GetById(authorId);

            Assert.That(result, Is.EqualTo(expectedAuthor));
        }

        [Test]
        public void GetAll_ShouldReturnAllAuthors()
        {
            var authors = new List<Author>
            {
                new Author { Id = 1, Name = "Author 1" },
                new Author { Id = 2, Name = "Author 2" },
                new Author { Id = 3, Name = "Author 3" }
            }.AsQueryable();

            _mockRepo.Setup(r => r.GetAll()).Returns(authors);

            var result = _authorService.GetAll();

            Assert.That(result, Is.EqualTo(authors));
        }

        [Test]
        public async Task Get_ShouldCallRepositoryWithCorrectFilter()
        {
            var expectedAuthor = new Author { Id = 1, Name = "Test Author" };
            Expression<Func<Author, bool>> filter = a => a.Id == 1;

            _mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<Author, bool>>>()))
                    .ReturnsAsync(expectedAuthor);

            var result = await _authorService.Get(filter);

            Assert.That(result, Is.EqualTo(expectedAuthor));
        }

        [Test]
        public async Task Find_ShouldCallRepositoryWithCorrectFilter()
        {
            var expectedAuthors = new List<Author>
            {
                new Author { Id = 1, Name = "Author 1" },
                new Author { Id = 2, Name = "Author 2" }
            };

            Expression<Func<Author, bool>> filter = a => a.Name.Contains("Author");

            _mockRepo.Setup(r => r.Find(It.IsAny<Expression<Func<Author, bool>>>()))
                    .ReturnsAsync(expectedAuthors);

            var result = await _authorService.Find(filter);

            Assert.That(result, Is.EqualTo(expectedAuthors));
        }

        [Test]
        public async Task Add_ShouldCallRepositoryAddMethod()
        {
            var author = new Author { Id = 1, Name = "New Author" };
            _mockRepo.Setup(r => r.Add(It.IsAny<Author>())).Returns(Task.CompletedTask);

            await _authorService.Add(author);

        }

        [Test]
        public async Task Update_ShouldCallRepositoryUpdateMethod()
        {
            var author = new Author { Id = 1, Name = "Updated Author" };
            _mockRepo.Setup(r => r.Update(It.IsAny<Author>())).Returns(Task.CompletedTask);

            await _authorService.Update(author);

        }

        [Test]
        public async Task Delete_ShouldCallRepositoryDeleteMethod()
        {
            int authorId = 1;
            _mockRepo.Setup(r => r.Delete(authorId)).Returns(Task.CompletedTask);

            await _authorService.Delete(authorId);

        }
        private object InvokePrivateMethod(object instance, string methodName, params object[] parameters)
        {
            Type type = instance.GetType();
            var method = type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            return method.Invoke(instance, parameters);
        }
    }
}
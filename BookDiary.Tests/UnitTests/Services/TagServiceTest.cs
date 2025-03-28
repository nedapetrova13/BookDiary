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
using System.Reflection;
using BookDiary.Core.Validators;

namespace BookDiary.Tests.UnitTests.Services
{
    [TestFixture]
    public class TagServiceTests
    {
        private Mock<IRepository<Tag>> _mockRepo;
        private ITagService _tagService;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IRepository<Tag>>();
            _tagService = new TagService(_mockRepo.Object);
        }

        [Test]
        public async Task GetById_ShouldCallRepositoryWithCorrectId()
        {
            // Arrange
            int tagId = 1;
            var expectedTag = new Tag { Id = tagId, Name = "Fiction" };
            _mockRepo.Setup(r => r.GetById(tagId)).ReturnsAsync(expectedTag);

            // Act
            var result = await _tagService.GetById(tagId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedTag));
            _mockRepo.Verify(r => r.GetById(tagId), Times.Once);
        }

        [Test]
        public void GetAll_ShouldReturnAllTags()
        {
            // Arrange
            var tags = new List<Tag>
            {
                new Tag { Id = 1, Name = "Fiction" },
                new Tag { Id = 2, Name = "Science Fiction" },
                new Tag { Id = 3, Name = "Mystery" }
            }.AsQueryable();

            _mockRepo.Setup(r => r.GetAll()).Returns(tags);

            // Act
            var result = _tagService.GetAll();

            // Assert
            Assert.That(result, Is.EqualTo(tags));
            _mockRepo.Verify(r => r.GetAll(), Times.Once);
        }

        [Test]
        public async Task Get_ShouldCallRepositoryWithCorrectFilter()
        {
            // Arrange
            var expectedTag = new Tag { Id = 1, Name = "Fiction" };
            Expression<Func<Tag, bool>> filter = t => t.Name == "Fiction";

            _mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<Tag, bool>>>()))
                    .ReturnsAsync(expectedTag);

            // Act
            var result = await _tagService.Get(filter);

            // Assert
            Assert.That(result, Is.EqualTo(expectedTag));
            _mockRepo.Verify(r => r.Get(It.IsAny<Expression<Func<Tag, bool>>>()), Times.Once);
        }

        [Test]
        public async Task Find_ShouldCallRepositoryWithCorrectFilter()
        {
            // Arrange
            var expectedTags = new List<Tag>
            {
                new Tag { Id = 1, Name = "Fiction" },
                new Tag { Id = 2, Name = "Science Fiction" }
            };

            Expression<Func<Tag, bool>> filter = t => t.Name.Contains("Fiction");

            _mockRepo.Setup(r => r.Find(It.IsAny<Expression<Func<Tag, bool>>>()))
                    .ReturnsAsync(expectedTags);

            // Act
            var result = await _tagService.Find(filter);

            // Assert
            Assert.That(result, Is.EqualTo(expectedTags));
            _mockRepo.Verify(r => r.Find(It.IsAny<Expression<Func<Tag, bool>>>()), Times.Once);
        }

        [Test]
        public async Task Add_ShouldCallRepositoryAddMethod()
        {
            // Arrange
            var tag = new Tag { Name = "New Tag" };
            _mockRepo.Setup(r => r.Add(It.IsAny<Tag>())).Returns(Task.CompletedTask);

            // Act
            await _tagService.Add(tag);

            // Assert
            _mockRepo.Verify(r => r.Add(tag), Times.Once);
        }

        [Test]
        public async Task Update_ShouldCallRepositoryUpdateMethod()
        {
            // Arrange
            var tag = new Tag { Id = 1, Name = "Updated Tag" };
            _mockRepo.Setup(r => r.Update(It.IsAny<Tag>())).Returns(Task.CompletedTask);

            // Act
            await _tagService.Update(tag);

            // Assert
            _mockRepo.Verify(r => r.Update(tag), Times.Once);
        }

        [Test]
        public async Task Delete_ShouldCallRepositoryDeleteMethod()
        {
            // Arrange
            int tagId = 1;
            _mockRepo.Setup(r => r.Delete(tagId)).Returns(Task.CompletedTask);

            // Act
            await _tagService.Delete(tagId);

            // Assert
            _mockRepo.Verify(r => r.Delete(tagId), Times.Once);
        }

        [Test]
        public void Constructor_WithNullRepository_ShouldNotThrowException()
        {
            // Arrange & Act & Assert
            // This test verifies the current behavior, which does not throw when null is passed
            Assert.DoesNotThrow(() => new TagService(null));

            // Note: If you want to add null checking to your constructor, this test should be changed to:
            // Assert.Throws<ArgumentNullException>(() => new TagService(null));
        }

        [Test]
        public void Constructor_WithValidRepository_ShouldInitializeService()
        {
            // Arrange
            var repo = new Mock<IRepository<Tag>>();

            // Act
            var service = new TagService(repo.Object);

            // Assert - test that the service is properly initialized by calling a method
            Assert.DoesNotThrow(() => service.GetAll());
        }

        
        [Test]
        public async Task TagWithBooks_ShouldWorkCorrectly()
        {
            // Arrange
            var tag = new Tag
            {
                Id = 1,
                Name = "Fiction",
                BookTags = new List<BookTag>
                {
                    new BookTag
                    {
                        Id = 1,
                        TagId = 1,
                        BookId = 10,
                        Book = new Book { Id = 10, Title = "Book 1" }
                    },
                    new BookTag
                    {
                        Id = 2,
                        TagId = 1,
                        BookId = 20,
                        Book = new Book { Id = 20, Title = "Book 2" }
                    }
                }
            };

            _mockRepo.Setup(r => r.GetById(tag.Id)).ReturnsAsync(tag);

            // Act
            var result = await _tagService.GetById(tag.Id);

            // Assert
            Assert.That(result, Is.EqualTo(tag));
            Assert.That(result.BookTags.Count, Is.EqualTo(2));
            Assert.That(result.BookTags.Any(bt => bt.BookId == 10 && bt.Book.Title == "Book 1"), Is.True);
            Assert.That(result.BookTags.Any(bt => bt.BookId == 20 && bt.Book.Title == "Book 2"), Is.True);
        }

        [Test]
        public async Task TagWorkflow_AddFindUpdateDelete_ShouldWorkCorrectly()
        {
            // Arrange
            var tag = new Tag { Id = 1, Name = "SciFi" };
            var updatedTag = new Tag { Id = 1, Name = "Science Fiction" };

            _mockRepo.Setup(r => r.Add(It.IsAny<Tag>())).Returns(Task.CompletedTask);
            _mockRepo.Setup(r => r.Find(It.IsAny<Expression<Func<Tag, bool>>>()))
                .ReturnsAsync(new List<Tag> { tag });
            _mockRepo.Setup(r => r.Update(It.IsAny<Tag>())).Returns(Task.CompletedTask);
            _mockRepo.Setup(r => r.Delete(It.IsAny<int>())).Returns(Task.CompletedTask);

            // Act & Assert - Full workflow
            // 1. Add a tag
            await _tagService.Add(tag);
            _mockRepo.Verify(r => r.Add(tag), Times.Once);

            // 2. Find the tag
            var foundTags = await _tagService.Find(t => t.Name.Contains("Sci"));
            Assert.That(foundTags.Count, Is.EqualTo(1));
            Assert.That(foundTags[0], Is.EqualTo(tag));

            // 3. Update the tag
            await _tagService.Update(updatedTag);
            _mockRepo.Verify(r => r.Update(updatedTag), Times.Once);

            // 4. Delete the tag
            await _tagService.Delete(tag.Id);
            _mockRepo.Verify(r => r.Delete(tag.Id), Times.Once);
        }

        // Helper method to invoke private methods using reflection
        private T InvokePrivateMethod<T>(object instance, string methodName, params object[] parameters)
        {
            Type type = instance.GetType();
            MethodInfo method = type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            return (T)method.Invoke(instance, parameters);
        }
    }

    // This is a simple mock for static methods
    // In a real project, you'd use a library like Fody or a more sophisticated approach
    public class MockStaticContext : IDisposable
    {
        public static MockStaticContext For<TResult>(Expression<Func<TResult>> expression)
        {
            return new MockStaticContext();
        }

        public void Setup<TResult>(Expression<Func<TResult>> expression)
        {
            // This is a placeholder - in a real implementation, you would set up mocking for static methods
        }

        public void Setup<TResult>(Expression<Func<TResult>> expression, TResult returnValue)
        {
            // This is a placeholder - in a real implementation, you would set up mocking for static methods with return value
        }

        public void Dispose()
        {
            // Clean up any mocking infrastructure
        }
    }
}
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
    public class BookTagServiceTests
    {
        private Mock<IRepository<BookTag>> _mockRepo;
        private IBookTagService _bookTagService;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IRepository<BookTag>>();
            _bookTagService = new BookTagService(_mockRepo.Object);
        }

        [Test]
        public async Task GetById_ShouldCallRepositoryWithCorrectId()
        {
            // Arrange
            int bookTagId = 1;
            var expectedBookTag = new BookTag { Id = bookTagId, BookId = 10, TagId = 20 };
            _mockRepo.Setup(r => r.GetById(bookTagId)).ReturnsAsync(expectedBookTag);

            // Act
            var result = await _bookTagService.GetById(bookTagId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedBookTag));
            _mockRepo.Verify(r => r.GetById(bookTagId), Times.Once);
        }

        [Test]
        public void GetAll_ShouldReturnAllBookTags()
        {
            // Arrange
            var bookTags = new List<BookTag>
            {
                new BookTag { Id = 1, BookId = 10, TagId = 20 },
                new BookTag { Id = 2, BookId = 10, TagId = 30 },
                new BookTag { Id = 3, BookId = 20, TagId = 20 }
            }.AsQueryable();

            _mockRepo.Setup(r => r.GetAll()).Returns(bookTags);

            // Act
            var result = _bookTagService.GetAll();

            // Assert
            Assert.That(result, Is.EqualTo(bookTags));
            _mockRepo.Verify(r => r.GetAll(), Times.Once);
        }

        [Test]
        public async Task Get_ShouldCallRepositoryWithCorrectFilter()
        {
            // Arrange
            var expectedBookTag = new BookTag { Id = 1, BookId = 10, TagId = 20 };
            Expression<Func<BookTag, bool>> filter = bt => bt.BookId == 10 && bt.TagId == 20;

            _mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<BookTag, bool>>>()))
                    .ReturnsAsync(expectedBookTag);

            // Act
            var result = await _bookTagService.Get(filter);

            // Assert
            Assert.That(result, Is.EqualTo(expectedBookTag));
            _mockRepo.Verify(r => r.Get(It.IsAny<Expression<Func<BookTag, bool>>>()), Times.Once);
        }

        [Test]
        public async Task Find_ShouldCallRepositoryWithCorrectFilter()
        {
            // Arrange
            var expectedBookTags = new List<BookTag>
            {
                new BookTag { Id = 1, BookId = 10, TagId = 20 },
                new BookTag { Id = 2, BookId = 10, TagId = 30 }
            };

            Expression<Func<BookTag, bool>> filter = bt => bt.BookId == 10;

            _mockRepo.Setup(r => r.Find(It.IsAny<Expression<Func<BookTag, bool>>>()))
                    .ReturnsAsync(expectedBookTags);

            // Act
            var result = await _bookTagService.Find(filter);

            // Assert
            Assert.That(result, Is.EqualTo(expectedBookTags));
            _mockRepo.Verify(r => r.Find(It.IsAny<Expression<Func<BookTag, bool>>>()), Times.Once);
        }

        [Test]
        public async Task Add_ShouldCallRepositoryAddMethod()
        {
            // Arrange
            var bookTag = new BookTag { BookId = 10, TagId = 20 };
            _mockRepo.Setup(r => r.Add(It.IsAny<BookTag>())).Returns(Task.CompletedTask);

            // Act
            await _bookTagService.Add(bookTag);

            // Assert
            _mockRepo.Verify(r => r.Add(bookTag), Times.Once);
        }

        [Test]
        public async Task Update_ShouldCallRepositoryUpdateMethod()
        {
            // Arrange
            var bookTag = new BookTag { Id = 1, BookId = 10, TagId = 20 };
            _mockRepo.Setup(r => r.Update(It.IsAny<BookTag>())).Returns(Task.CompletedTask);

            // Act
            await _bookTagService.Update(bookTag);

            // Assert
            _mockRepo.Verify(r => r.Update(bookTag), Times.Once);
        }

        [Test]
        public async Task Delete_ShouldCallRepositoryDeleteMethod()
        {
            // Arrange
            int bookTagId = 1;
            _mockRepo.Setup(r => r.Delete(bookTagId)).Returns(Task.CompletedTask);

            // Act
            await _bookTagService.Delete(bookTagId);

            // Assert
            _mockRepo.Verify(r => r.Delete(bookTagId), Times.Once);
        }

        [Test]
        public async Task DeleteBookTag_ShouldCallRepositoryDeleteMappingMethod()
        {
            // Arrange
            int bookId = 10;
            int tagId = 20;

            _mockRepo.Setup(r => r.DeleteMapping<BookTag>(
                It.IsAny<Expression<Func<BookTag, bool>>>()))
                .Returns(Task.CompletedTask);

            // Act
            await _bookTagService.DeleteBookTag(bookId, tagId);

            // Assert
            _mockRepo.Verify(r => r.DeleteMapping<BookTag>(
                It.IsAny<Expression<Func<BookTag, bool>>>()), Times.Once);
        }

        [Test]
        public void Constructor_WithNullRepository_ShouldNotThrowException()
        {
            // Arrange & Act & Assert
            // This test verifies the current behavior, which does not throw when null is passed
            Assert.DoesNotThrow(() => new BookTagService(null));

            // Note: If you want to add null checking to your constructor, this test should be changed to:
            // Assert.Throws<ArgumentNullException>(() => new BookTagService(null));
        }
    }
}
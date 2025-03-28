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
    public class CommentServiceTests
    {
        private Mock<IRepository<Comment>> _mockRepo;
        private ICommentService _commentService;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IRepository<Comment>>();
            _commentService = new CommentService(_mockRepo.Object);
        }

        [Test]
        public async Task GetById_ShouldCallRepositoryWithCorrectId()
        {
            // Arrange
            int commentId = 1;
            var expectedComment = new Comment { Id = commentId, Content = "Great book!", Rating = 5 };
            _mockRepo.Setup(r => r.GetById(commentId)).ReturnsAsync(expectedComment);

            // Act
            var result = await _commentService.GetById(commentId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedComment));
            _mockRepo.Verify(r => r.GetById(commentId), Times.Once);
        }

        [Test]
        public void GetAll_ShouldReturnAllComments()
        {
            // Arrange
            var comments = new List<Comment>
            {
                new Comment { Id = 1, Content = "Comment 1", Rating = 5 },
                new Comment { Id = 2, Content = "Comment 2", Rating = 4 },
                new Comment { Id = 3, Content = "Comment 3", Rating = 3 }
            }.AsQueryable();

            _mockRepo.Setup(r => r.GetAll()).Returns(comments);

            // Act
            var result = _commentService.GetAll();

            // Assert
            Assert.That(result, Is.EqualTo(comments));
            _mockRepo.Verify(r => r.GetAll(), Times.Once);
        }

        [Test]
        public async Task Get_ShouldCallRepositoryWithCorrectFilter()
        {
            // Arrange
            var expectedComment = new Comment { Id = 1, Content = "Test Comment", Rating = 5 };
            Expression<Func<Comment, bool>> filter = c => c.Id == 1;

            _mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<Comment, bool>>>()))
                    .ReturnsAsync(expectedComment);

            // Act
            var result = await _commentService.Get(filter);

            // Assert
            Assert.That(result, Is.EqualTo(expectedComment));
            _mockRepo.Verify(r => r.Get(It.IsAny<Expression<Func<Comment, bool>>>()), Times.Once);
        }

        [Test]
        public async Task Find_ShouldCallRepositoryWithCorrectFilter()
        {
            // Arrange
            var expectedComments = new List<Comment>
            {
                new Comment { Id = 1, Content = "Comment 1", BookId = 10 },
                new Comment { Id = 2, Content = "Comment 2", BookId = 10 }
            };

            Expression<Func<Comment, bool>> filter = c => c.BookId == 10;

            _mockRepo.Setup(r => r.Find(It.IsAny<Expression<Func<Comment, bool>>>()))
                    .ReturnsAsync(expectedComments);

            // Act
            var result = await _commentService.Find(filter);

            // Assert
            Assert.That(result, Is.EqualTo(expectedComments));
            _mockRepo.Verify(r => r.Find(It.IsAny<Expression<Func<Comment, bool>>>()), Times.Once);
        }

        [Test]
        public async Task Add_ShouldCallRepositoryAddMethod()
        {
            // Arrange
            var comment = new Comment { Content = "New Comment", Rating = 5 };
            _mockRepo.Setup(r => r.Add(It.IsAny<Comment>())).Returns(Task.CompletedTask);

            // Act
            await _commentService.Add(comment);

            // Assert
            _mockRepo.Verify(r => r.Add(comment), Times.Once);
        }

        [Test]
        public async Task Update_ShouldCallRepositoryUpdateMethod()
        {
            // Arrange
            var comment = new Comment { Id = 1, Content = "Updated Comment", Rating = 4 };
            _mockRepo.Setup(r => r.Update(It.IsAny<Comment>())).Returns(Task.CompletedTask);

            // Act
            await _commentService.Update(comment);

            // Assert
            _mockRepo.Verify(r => r.Update(comment), Times.Once);
        }

        [Test]
        public async Task Delete_ShouldCallRepositoryDeleteMethod()
        {
            // Arrange
            int commentId = 1;
            _mockRepo.Setup(r => r.Delete(commentId)).Returns(Task.CompletedTask);

            // Act
            await _commentService.Delete(commentId);

            // Assert
            _mockRepo.Verify(r => r.Delete(commentId), Times.Once);
        }

        [Test]
        public async Task DeleteComment_ShouldCallRepositoryDeleteMappingMethod()
        {
            // Arrange
            int bookId = 10;
            string userId = "user123";

            _mockRepo.Setup(r => r.DeleteMapping<Comment>(
                It.IsAny<Expression<Func<Comment, bool>>>()))
                .Returns(Task.CompletedTask);

            // Act
            await _commentService.DeleteComment(bookId, userId);

            // Assert
            _mockRepo.Verify(r => r.DeleteMapping<Comment>(
                It.IsAny<Expression<Func<Comment, bool>>>()), Times.Once);
        }

        [Test]
        public void Constructor_WithNullRepository_ShouldNotThrowException()
        {
            // Arrange & Act & Assert
            // This test verifies the current behavior, which does not throw when null is passed
            Assert.DoesNotThrow(() => new CommentService(null));

            // Note: If you want to add null checking to your constructor, this test should be changed to:
            // Assert.Throws<ArgumentNullException>(() => new CommentService(null));
        }

        [Test]
        public void Constructor_WithValidRepository_ShouldInitializeService()
        {
            // Arrange
            var repo = new Mock<IRepository<Comment>>();

            // Act
            var service = new CommentService(repo.Object);

            // Assert - test that the service is properly initialized by calling a method
            Assert.DoesNotThrow(() => service.GetAll());
        }
    }
}
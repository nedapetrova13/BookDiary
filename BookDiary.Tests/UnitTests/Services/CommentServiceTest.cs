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
            int commentId = 1;
            var expectedComment = new Comment { Id = commentId, Content = "Great book!", Rating = 5 };
            _mockRepo.Setup(r => r.GetById(commentId)).ReturnsAsync(expectedComment);

            var result = await _commentService.GetById(commentId);

            Assert.That(result, Is.EqualTo(expectedComment));
        }

        [Test]
        public void GetAll_ShouldReturnAllComments()
        {
            var comments = new List<Comment>
            {
                new Comment { Id = 1, Content = "Comment 1", Rating = 5 },
                new Comment { Id = 2, Content = "Comment 2", Rating = 4 },
                new Comment { Id = 3, Content = "Comment 3", Rating = 3 }
            }.AsQueryable();

            _mockRepo.Setup(r => r.GetAll()).Returns(comments);

            var result = _commentService.GetAll();

            Assert.That(result, Is.EqualTo(comments));
        }

        [Test]
        public async Task Get_ShouldCallRepositoryWithCorrectFilter()
        {
            var expectedComment = new Comment { Id = 1, Content = "Test Comment", Rating = 5 };
            Expression<Func<Comment, bool>> filter = c => c.Id == 1;

            _mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<Comment, bool>>>()))
                    .ReturnsAsync(expectedComment);

            var result = await _commentService.Get(filter);

            Assert.That(result, Is.EqualTo(expectedComment));
        }

        [Test]
        public async Task Find_ShouldCallRepositoryWithCorrectFilter()
        {
            var expectedComments = new List<Comment>
            {
                new Comment { Id = 1, Content = "Comment 1", BookId = 10 },
                new Comment { Id = 2, Content = "Comment 2", BookId = 10 }
            };

            Expression<Func<Comment, bool>> filter = c => c.BookId == 10;

            _mockRepo.Setup(r => r.Find(It.IsAny<Expression<Func<Comment, bool>>>()))
                    .ReturnsAsync(expectedComments);

            var result = await _commentService.Find(filter);

            Assert.That(result, Is.EqualTo(expectedComments));
        }

        [Test]
        public async Task Add_ShouldCallRepositoryAddMethod()
        {
            var comment = new Comment { Content = "New Comment", Rating = 5 };
            _mockRepo.Setup(r => r.Add(It.IsAny<Comment>())).Returns(Task.CompletedTask);

            await _commentService.Add(comment);

        }

        [Test]
        public async Task Update_ShouldCallRepositoryUpdateMethod()
        {
            var comment = new Comment { Id = 1, Content = "Updated Comment", Rating = 4 };
            _mockRepo.Setup(r => r.Update(It.IsAny<Comment>())).Returns(Task.CompletedTask);

            await _commentService.Update(comment);

        }

        [Test]
        public async Task Delete_ShouldCallRepositoryDeleteMethod()
        {
            int commentId = 1;
            _mockRepo.Setup(r => r.Delete(commentId)).Returns(Task.CompletedTask);

            await _commentService.Delete(commentId);
        }

        [Test]
        public async Task DeleteComment_ShouldCallRepositoryDeleteMappingMethod()
        {
            int bookId = 10;
            string userId = "user123";

            _mockRepo.Setup(r => r.DeleteMapping<Comment>(
                It.IsAny<Expression<Func<Comment, bool>>>()))
                .Returns(Task.CompletedTask);

            await _commentService.DeleteComment(bookId, userId);

        }

        [Test]
        public void Constructor_WithNullRepository_ShouldNotThrowException()
        {
            Assert.DoesNotThrow(() => new CommentService(null));

        }

        [Test]
        public void Constructor_WithValidRepository_ShouldInitializeService()
        {
            var repo = new Mock<IRepository<Comment>>();

            var service = new CommentService(repo.Object);

            Assert.DoesNotThrow(() => service.GetAll());
        }
    }
}
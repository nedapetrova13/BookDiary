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
            int bookTagId = 1;
            var expectedBookTag = new BookTag { Id = bookTagId, BookId = 10, TagId = 20 };
            _mockRepo.Setup(r => r.GetById(bookTagId)).ReturnsAsync(expectedBookTag);

            var result = await _bookTagService.GetById(bookTagId);

            Assert.That(result, Is.EqualTo(expectedBookTag));
        }

        [Test]
        public void GetAll_ShouldReturnAllBookTags()
        {
            var bookTags = new List<BookTag>
            {
                new BookTag { Id = 1, BookId = 10, TagId = 20 },
                new BookTag { Id = 2, BookId = 10, TagId = 30 },
                new BookTag { Id = 3, BookId = 20, TagId = 20 }
            }.AsQueryable();

            _mockRepo.Setup(r => r.GetAll()).Returns(bookTags);

            var result = _bookTagService.GetAll();

            Assert.That(result, Is.EqualTo(bookTags));
        }

        [Test]
        public async Task Get_ShouldCallRepositoryWithCorrectFilter()
        {
            var expectedBookTag = new BookTag { Id = 1, BookId = 10, TagId = 20 };
            Expression<Func<BookTag, bool>> filter = bt => bt.BookId == 10 && bt.TagId == 20;

            _mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<BookTag, bool>>>()))
                    .ReturnsAsync(expectedBookTag);

            var result = await _bookTagService.Get(filter);

            Assert.That(result, Is.EqualTo(expectedBookTag));
        }

        [Test]
        public async Task Find_ShouldCallRepositoryWithCorrectFilter()
        {
            var expectedBookTags = new List<BookTag>
            {
                new BookTag { Id = 1, BookId = 10, TagId = 20 },
                new BookTag { Id = 2, BookId = 10, TagId = 30 }
            };

            Expression<Func<BookTag, bool>> filter = bt => bt.BookId == 10;

            _mockRepo.Setup(r => r.Find(It.IsAny<Expression<Func<BookTag, bool>>>()))
                    .ReturnsAsync(expectedBookTags);

            var result = await _bookTagService.Find(filter);

            Assert.That(result, Is.EqualTo(expectedBookTags));
        }

        [Test]
        public async Task Add_ShouldCallRepositoryAddMethod()
        {
            var bookTag = new BookTag { BookId = 10, TagId = 20 };
            _mockRepo.Setup(r => r.Add(It.IsAny<BookTag>())).Returns(Task.CompletedTask);

            await _bookTagService.Add(bookTag);

        }

        [Test]
        public async Task Update_ShouldCallRepositoryUpdateMethod()
        {
            var bookTag = new BookTag { Id = 1, BookId = 10, TagId = 20 };
            _mockRepo.Setup(r => r.Update(It.IsAny<BookTag>())).Returns(Task.CompletedTask);

            await _bookTagService.Update(bookTag);
        }

        [Test]
        public async Task Delete_ShouldCallRepositoryDeleteMethod()
        {
            int bookTagId = 1;
            _mockRepo.Setup(r => r.Delete(bookTagId)).Returns(Task.CompletedTask);

            await _bookTagService.Delete(bookTagId);
        }

        [Test]
        public async Task DeleteBookTag_ShouldCallRepositoryDeleteMappingMethod()
        {
            int bookId = 10;
            int tagId = 20;

            _mockRepo.Setup(r => r.DeleteMapping<BookTag>(
                It.IsAny<Expression<Func<BookTag, bool>>>()))
                .Returns(Task.CompletedTask);

            await _bookTagService.DeleteBookTag(bookId, tagId);
        }

        [Test]
        public void Constructor_WithNullRepository_ShouldNotThrowException()
        {
            Assert.DoesNotThrow(() => new BookTagService(null));
        }
    }
}
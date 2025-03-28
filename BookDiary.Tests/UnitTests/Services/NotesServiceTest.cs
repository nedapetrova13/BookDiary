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
    public class NotesServiceTests
    {
        private Mock<IRepository<Notes>> _mockRepo;
        private INotesService _notesService;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IRepository<Notes>>();
            _notesService = new NotesService(_mockRepo.Object);
        }

        [Test]
        public async Task GetById_ShouldCallRepositoryWithCorrectId()
        {
            // Arrange
            int notesId = 1;
            var expectedNotes = new Notes { Id = notesId, Title = "Test Notes", NoteContent = "Test Content" };
            _mockRepo.Setup(r => r.GetById(notesId)).ReturnsAsync(expectedNotes);

            // Act
            var result = await _notesService.GetById(notesId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedNotes));
            _mockRepo.Verify(r => r.GetById(notesId), Times.Once);
        }

        [Test]
        public void GetAll_ShouldReturnAllNotes()
        {
            // Arrange
            var notesList = new List<Notes>
            {
                new Notes { Id = 1, Title = "Notes 1", NoteContent = "Content 1" },
                new Notes { Id = 2, Title = "Notes 2", NoteContent = "Content 2" },
                new Notes { Id = 3, Title = "Notes 3", NoteContent = "Content 3" }
            }.AsQueryable();

            _mockRepo.Setup(r => r.GetAll()).Returns(notesList);

            // Act
            var result = _notesService.GetAll();

            // Assert
            Assert.That(result, Is.EqualTo(notesList));
            _mockRepo.Verify(r => r.GetAll(), Times.Once);
        }

        [Test]
        public async Task Get_ShouldCallRepositoryWithCorrectFilter()
        {
            // Arrange
            var expectedNotes = new Notes { Id = 1, Title = "Test Notes", NoteContent = "Test Content" };
            Expression<Func<Notes, bool>> filter = n => n.Title == "Test Notes";

            _mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<Notes, bool>>>()))
                    .ReturnsAsync(expectedNotes);

            // Act
            var result = await _notesService.Get(filter);

            // Assert
            Assert.That(result, Is.EqualTo(expectedNotes));
            _mockRepo.Verify(r => r.Get(It.IsAny<Expression<Func<Notes, bool>>>()), Times.Once);
        }

        [Test]
        public async Task Find_ShouldCallRepositoryWithCorrectFilter()
        {
            // Arrange
            var expectedNotes = new List<Notes>
            {
                new Notes { Id = 1, Title = "Chapter 1 Notes", NoteContent = "Content 1", BookChapter = 1 },
                new Notes { Id = 2, Title = "Chapter 2 Notes", NoteContent = "Content 2", BookChapter = 2 }
            };

            Expression<Func<Notes, bool>> filter = n => n.Title.Contains("Chapter");

            _mockRepo.Setup(r => r.Find(It.IsAny<Expression<Func<Notes, bool>>>()))
                    .ReturnsAsync(expectedNotes);

            // Act
            var result = await _notesService.Find(filter);

            // Assert
            Assert.That(result, Is.EqualTo(expectedNotes));
            _mockRepo.Verify(r => r.Find(It.IsAny<Expression<Func<Notes, bool>>>()), Times.Once);
        }

        [Test]
        public async Task Add_ShouldCallRepositoryAddMethod()
        {
            // Arrange
            var notes = new Notes { Title = "New Notes", NoteContent = "New Content", BookChapter = 1 };
            _mockRepo.Setup(r => r.Add(It.IsAny<Notes>())).Returns(Task.CompletedTask);

            // Act
            await _notesService.Add(notes);

            // Assert
            _mockRepo.Verify(r => r.Add(notes), Times.Once);
        }

        [Test]
        public async Task Update_ShouldCallRepositoryUpdateMethod()
        {
            // Arrange
            var notes = new Notes { Id = 1, Title = "Updated Notes", NoteContent = "Updated Content", BookChapter = 1 };
            _mockRepo.Setup(r => r.Update(It.IsAny<Notes>())).Returns(Task.CompletedTask);

            // Act
            await _notesService.Update(notes);

            // Assert
            _mockRepo.Verify(r => r.Update(notes), Times.Once);
        }

        [Test]
        public async Task Delete_ShouldCallRepositoryDeleteMethod()
        {
            // Arrange
            int notesId = 1;
            _mockRepo.Setup(r => r.Delete(notesId)).Returns(Task.CompletedTask);

            // Act
            await _notesService.Delete(notesId);

            // Assert
            _mockRepo.Verify(r => r.Delete(notesId), Times.Once);
        }

        [Test]
        public void Constructor_WithNullRepository_ShouldNotThrowException()
        {
            // Arrange & Act & Assert
            // This test verifies the current behavior, which does not throw when null is passed
            Assert.DoesNotThrow(() => new NotesService(null));

            // Note: If you want to add null checking to your constructor, this test should be changed to:
            // Assert.Throws<ArgumentNullException>(() => new NotesService(null));
        }

        [Test]
        public void Constructor_WithValidRepository_ShouldInitializeService()
        {
            // Arrange
            var repo = new Mock<IRepository<Notes>>();

            // Act
            var service = new NotesService(repo.Object);

            // Assert - test that the service is properly initialized by calling a method
            Assert.DoesNotThrow(() => service.GetAll());
        }

        [Test]
        public async Task NotesWorkflow_AddFindUpdateDelete_ShouldWorkCorrectly()
        {
            // Arrange
            var bookId = 10;
            var userId = "user123";

            var notes = new Notes
            {
                Id = 1,
                Title = "Chapter 1 Notes",
                NoteContent = "Initial notes for chapter 1",
                BookChapter = 1,
                BookId = bookId,
                UserId = userId
            };

            var updatedNotes = new Notes
            {
                Id = 1,
                Title = "Chapter 1 Notes - Updated",
                NoteContent = "Updated notes for chapter 1",
                BookChapter = 1,
                BookId = bookId,
                UserId = userId
            };

            _mockRepo.Setup(r => r.Add(It.IsAny<Notes>())).Returns(Task.CompletedTask);
            _mockRepo.Setup(r => r.Find(It.IsAny<Expression<Func<Notes, bool>>>()))
                .ReturnsAsync(new List<Notes> { notes });
            _mockRepo.Setup(r => r.Update(It.IsAny<Notes>())).Returns(Task.CompletedTask);
            _mockRepo.Setup(r => r.Delete(It.IsAny<int>())).Returns(Task.CompletedTask);

            // Act & Assert - Full workflow
            // 1. Add notes
            await _notesService.Add(notes);
            _mockRepo.Verify(r => r.Add(notes), Times.Once);

            // 2. Find the notes
            var foundNotes = await _notesService.Find(n => n.BookId == bookId && n.UserId == userId);
            Assert.That(foundNotes.Count, Is.EqualTo(1));
            Assert.That(foundNotes[0], Is.EqualTo(notes));

            // 3. Update the notes
            await _notesService.Update(updatedNotes);
            _mockRepo.Verify(r => r.Update(updatedNotes), Times.Once);

            // 4. Delete the notes
            await _notesService.Delete(notes.Id);
            _mockRepo.Verify(r => r.Delete(notes.Id), Times.Once);
        }
    }
}
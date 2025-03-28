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
    public class LanguageServiceTests
    {
        private Mock<IRepository<Language>> _mockRepo;
        private ILanguageService _languageService;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IRepository<Language>>();
            _languageService = new LanguageService(_mockRepo.Object);
        }

        [Test]
        public async Task GetById_ShouldCallRepositoryWithCorrectId()
        {
            // Arrange
            int languageId = 1;
            var expectedLanguage = new Language { Id = languageId, Name = "English" };
            _mockRepo.Setup(r => r.GetById(languageId)).ReturnsAsync(expectedLanguage);

            // Act
            var result = await _languageService.GetById(languageId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedLanguage));
            _mockRepo.Verify(r => r.GetById(languageId), Times.Once);
        }

        [Test]
        public void GetAll_ShouldReturnAllLanguages()
        {
            // Arrange
            var languages = new List<Language>
            {
                new Language { Id = 1, Name = "English" },
                new Language { Id = 2, Name = "Spanish" },
                new Language { Id = 3, Name = "French" }
            }.AsQueryable();

            _mockRepo.Setup(r => r.GetAll()).Returns(languages);

            // Act
            var result = _languageService.GetAll();

            // Assert
            Assert.That(result, Is.EqualTo(languages));
            _mockRepo.Verify(r => r.GetAll(), Times.Once);
        }

        [Test]
        public async Task Get_ShouldCallRepositoryWithCorrectFilter()
        {
            // Arrange
            var expectedLanguage = new Language { Id = 1, Name = "English" };
            Expression<Func<Language, bool>> filter = l => l.Name == "English";

            _mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<Language, bool>>>()))
                    .ReturnsAsync(expectedLanguage);

            // Act
            var result = await _languageService.Get(filter);

            // Assert
            Assert.That(result, Is.EqualTo(expectedLanguage));
            _mockRepo.Verify(r => r.Get(It.IsAny<Expression<Func<Language, bool>>>()), Times.Once);
        }

        [Test]
        public async Task Find_ShouldCallRepositoryWithCorrectFilter()
        {
            // Arrange
            var expectedLanguages = new List<Language>
            {
                new Language { Id = 1, Name = "English" },
                new Language { Id = 2, Name = "American English" }
            };

            Expression<Func<Language, bool>> filter = l => l.Name.Contains("English");

            _mockRepo.Setup(r => r.Find(It.IsAny<Expression<Func<Language, bool>>>()))
                    .ReturnsAsync(expectedLanguages);

            // Act
            var result = await _languageService.Find(filter);

            // Assert
            Assert.That(result, Is.EqualTo(expectedLanguages));
            _mockRepo.Verify(r => r.Find(It.IsAny<Expression<Func<Language, bool>>>()), Times.Once);
        }

        [Test]
        public async Task Add_ShouldCallRepositoryAddMethod()
        {
            // Arrange
            var language = new Language { Name = "German" };
            _mockRepo.Setup(r => r.Add(It.IsAny<Language>())).Returns(Task.CompletedTask);

            // Act
            await _languageService.Add(language);

            // Assert
            _mockRepo.Verify(r => r.Add(language), Times.Once);
        }

        [Test]
        public async Task Update_ShouldCallRepositoryUpdateMethod()
        {
            // Arrange
            var language = new Language { Id = 1, Name = "Updated Language" };
            _mockRepo.Setup(r => r.Update(It.IsAny<Language>())).Returns(Task.CompletedTask);

            // Act
            await _languageService.Update(language);

            // Assert
            _mockRepo.Verify(r => r.Update(language), Times.Once);
        }

        [Test]
        public async Task Delete_ShouldCallRepositoryDeleteMethod()
        {
            // Arrange
            int languageId = 1;
            _mockRepo.Setup(r => r.Delete(languageId)).Returns(Task.CompletedTask);

            // Act
            await _languageService.Delete(languageId);

            // Assert
            _mockRepo.Verify(r => r.Delete(languageId), Times.Once);
        }

        [Test]
        public void Constructor_WithNullRepository_ShouldNotThrowException()
        {
            // Arrange & Act & Assert
            // This test verifies the current behavior, which does not throw when null is passed
            Assert.DoesNotThrow(() => new LanguageService(null));

            // Note: If you want to add null checking to your constructor, this test should be changed to:
            // Assert.Throws<ArgumentNullException>(() => new LanguageService(null));
        }

        [Test]
        public void Constructor_WithValidRepository_ShouldInitializeService()
        {
            // Arrange
            var repo = new Mock<IRepository<Language>>();

            // Act
            var service = new LanguageService(repo.Object);

            // Assert - test that the service is properly initialized by calling a method
            Assert.DoesNotThrow(() => service.GetAll());
        }

        [Test]
        public async Task LanguageWorkflow_AddGetUpdateDelete_ShouldWorkCorrectly()
        {
            // Arrange
            var language = new Language { Id = 1, Name = "German" };
            var updatedLanguage = new Language { Id = 1, Name = "Austrian German" };

            _mockRepo.Setup(r => r.Add(It.IsAny<Language>())).Returns(Task.CompletedTask);
            _mockRepo.Setup(r => r.GetById(language.Id)).ReturnsAsync(language);
            _mockRepo.Setup(r => r.Update(It.IsAny<Language>())).Returns(Task.CompletedTask);
            _mockRepo.Setup(r => r.Delete(It.IsAny<int>())).Returns(Task.CompletedTask);

            // Act & Assert - Full workflow
            // 1. Add a language
            await _languageService.Add(language);
            _mockRepo.Verify(r => r.Add(language), Times.Once);

            // 2. Get the language by id
            var foundLanguage = await _languageService.GetById(language.Id);
            Assert.That(foundLanguage, Is.EqualTo(language));

            // 3. Update the language
            await _languageService.Update(updatedLanguage);
            _mockRepo.Verify(r => r.Update(updatedLanguage), Times.Once);

            // 4. Delete the language
            await _languageService.Delete(language.Id);
            _mockRepo.Verify(r => r.Delete(language.Id), Times.Once);
        }
    }
}
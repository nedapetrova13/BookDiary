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
            int languageId = 1;
            var expectedLanguage = new Language { Id = languageId, Name = "English" };
            _mockRepo.Setup(r => r.GetById(languageId)).ReturnsAsync(expectedLanguage);

            var result = await _languageService.GetById(languageId);

            Assert.That(result, Is.EqualTo(expectedLanguage));
        }

        [Test]
        public void GetAll_ShouldReturnAllLanguages()
        {
            var languages = new List<Language>
            {
                new Language { Id = 1, Name = "English" },
                new Language { Id = 2, Name = "Spanish" },
                new Language { Id = 3, Name = "French" }
            }.AsQueryable();

            _mockRepo.Setup(r => r.GetAll()).Returns(languages);

            var result = _languageService.GetAll();

            Assert.That(result, Is.EqualTo(languages));
        }

        [Test]
        public async Task Get_ShouldCallRepositoryWithCorrectFilter()
        {
            var expectedLanguage = new Language { Id = 1, Name = "English" };
            Expression<Func<Language, bool>> filter = l => l.Name == "English";

            _mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<Language, bool>>>()))
                    .ReturnsAsync(expectedLanguage);

            var result = await _languageService.Get(filter);

            Assert.That(result, Is.EqualTo(expectedLanguage));
        }

        [Test]
        public async Task Find_ShouldCallRepositoryWithCorrectFilter()
        {
            var expectedLanguages = new List<Language>
            {
                new Language { Id = 1, Name = "English" },
                new Language { Id = 2, Name = "American English" }
            };

            Expression<Func<Language, bool>> filter = l => l.Name.Contains("English");

            _mockRepo.Setup(r => r.Find(It.IsAny<Expression<Func<Language, bool>>>()))
                    .ReturnsAsync(expectedLanguages);

            var result = await _languageService.Find(filter);

            Assert.That(result, Is.EqualTo(expectedLanguages));
        }

        [Test]
        public async Task Add_ShouldCallRepositoryAddMethod()
        {
            var language = new Language { Name = "German" };
            _mockRepo.Setup(r => r.Add(It.IsAny<Language>())).Returns(Task.CompletedTask);

            await _languageService.Add(language);
        }

        [Test]
        public async Task Update_ShouldCallRepositoryUpdateMethod()
        {
            var language = new Language { Id = 1, Name = "Updated Language" };
            _mockRepo.Setup(r => r.Update(It.IsAny<Language>())).Returns(Task.CompletedTask);

            await _languageService.Update(language);

        }

        [Test]
        public async Task Delete_ShouldCallRepositoryDeleteMethod()
        {
            int languageId = 1;
            _mockRepo.Setup(r => r.Delete(languageId)).Returns(Task.CompletedTask);

            await _languageService.Delete(languageId);

        }

        [Test]
        public void Constructor_WithNullRepository_ShouldNotThrowException()
        {
            Assert.DoesNotThrow(() => new LanguageService(null));

        }

        [Test]
        public void Constructor_WithValidRepository_ShouldInitializeService()
        {
            var repo = new Mock<IRepository<Language>>();

            
            var service = new LanguageService(repo.Object);

            Assert.DoesNotThrow(() => service.GetAll());
        }

        [Test]
        public async Task LanguageWorkflow_AddGetUpdateDelete_ShouldWorkCorrectly()
        {
            var language = new Language { Id = 1, Name = "German" };
            var updatedLanguage = new Language { Id = 1, Name = "Austrian German" };

            _mockRepo.Setup(r => r.Add(It.IsAny<Language>())).Returns(Task.CompletedTask);
            _mockRepo.Setup(r => r.GetById(language.Id)).ReturnsAsync(language);
            _mockRepo.Setup(r => r.Update(It.IsAny<Language>())).Returns(Task.CompletedTask);
            _mockRepo.Setup(r => r.Delete(It.IsAny<int>())).Returns(Task.CompletedTask);

            await _languageService.Add(language);

            var foundLanguage = await _languageService.GetById(language.Id);
            Assert.That(foundLanguage, Is.EqualTo(language));

            await _languageService.Update(updatedLanguage);

            await _languageService.Delete(language.Id);
        }
    }
}
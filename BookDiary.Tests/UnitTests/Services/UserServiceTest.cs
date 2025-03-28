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
    public class UserServiceTests
    {
        private Mock<IRepository<User>> _mockRepo;
        private IUserService _userService;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IRepository<User>>();
            _userService = new UserService(_mockRepo.Object);
        }

        [Test]
        public async Task GetById_ShouldCallRepositoryWithCorrectId()
        {
            // Arrange
            int userId = 1;
            var expectedUser = new User { Id = "user1", UserName = "testuser", Name = "Test User" };
            _mockRepo.Setup(r => r.GetById(userId)).ReturnsAsync(expectedUser);

            // Act
            var result = await _userService.GetById(userId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedUser));
            _mockRepo.Verify(r => r.GetById(userId), Times.Once);
        }

        [Test]
        public async Task GetByIdUser_ShouldCallRepositoryWithCorrectId()
        {
            // Arrange
            string userId = "user1";
            var expectedUser = new User { Id = userId, UserName = "testuser", Name = "Test User" };
            _mockRepo.Setup(r => r.GetByIdUser(userId)).ReturnsAsync(expectedUser);

            // Act
            var result = await _userService.GetByIdUser(userId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedUser));
            _mockRepo.Verify(r => r.GetByIdUser(userId), Times.Once);
        }

        [Test]
        public void GetAll_ShouldReturnAllUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = "user1", UserName = "user1", Name = "User One" },
                new User { Id = "user2", UserName = "user2", Name = "User Two" },
                new User { Id = "user3", UserName = "user3", Name = "User Three" }
            }.AsQueryable();

            _mockRepo.Setup(r => r.GetAll()).Returns(users);

            // Act
            var result = _userService.GetAll();

            // Assert
            Assert.That(result, Is.EqualTo(users));
            _mockRepo.Verify(r => r.GetAll(), Times.Once);
        }

        [Test]
        public void Get_ShouldThrowNotImplementedException()
        {
            // Arrange
            Expression<Func<User, bool>> filter = u => u.UserName == "testuser";

            // Act & Assert
            Assert.ThrowsAsync<NotImplementedException>(async () => await _userService.Get(filter));
        }

        [Test]
        public async Task Find_ShouldCallRepositoryWithCorrectFilter()
        {
            // Arrange
            var expectedUsers = new List<User>
            {
                new User { Id = "user1", UserName = "admin1", Name = "Admin One" },
                new User { Id = "user2", UserName = "admin2", Name = "Admin Two" }
            };

            Expression<Func<User, bool>> filter = u => u.UserName.Contains("admin");

            _mockRepo.Setup(r => r.Find(It.IsAny<Expression<Func<User, bool>>>()))
                    .ReturnsAsync(expectedUsers);

            // Act
            var result = await _userService.Find(filter);

            // Assert
            Assert.That(result, Is.EqualTo(expectedUsers));
            _mockRepo.Verify(r => r.Find(It.IsAny<Expression<Func<User, bool>>>()), Times.Once);
        }

        [Test]
        public async Task Add_ShouldCallRepositoryAddMethod()
        {
            // Arrange
            var user = new User { Id = "user1", UserName = "newuser", Name = "New User" };
            _mockRepo.Setup(r => r.Add(It.IsAny<User>())).Returns(Task.CompletedTask);

            // Act
            await _userService.Add(user);

            // Assert
            _mockRepo.Verify(r => r.Add(user), Times.Once);
        }

        [Test]
        public async Task Update_ShouldCallRepositoryUpdateMethod()
        {
            // Arrange
            var user = new User { Id = "user1", UserName = "updateduser", Name = "Updated User" };
            _mockRepo.Setup(r => r.Update(It.IsAny<User>())).Returns(Task.CompletedTask);

            // Act
            await _userService.Update(user);

            // Assert
            _mockRepo.Verify(r => r.Update(user), Times.Once);
        }

        [Test]
        public async Task Delete_ShouldCallRepositoryDeleteMethod()
        {
            // Arrange
            int userId = 1;
            _mockRepo.Setup(r => r.Delete(userId)).Returns(Task.CompletedTask);

            // Act
            await _userService.Delete(userId);

            // Assert
            _mockRepo.Verify(r => r.Delete(userId), Times.Once);
        }

        [Test]
        public async Task DeleteUser_ShouldCallRepositoryDeleteUserMethod()
        {
            // Arrange
            string userId = "user1";
            _mockRepo.Setup(r => r.DeleteUser(userId)).Returns(Task.CompletedTask);

            // Act
            await _userService.DeleteUser(userId);

            // Assert
            _mockRepo.Verify(r => r.DeleteUser(userId), Times.Once);
        }

        [Test]
        public void Constructor_WithNullRepository_ShouldNotThrowException()
        {
            // Arrange & Act & Assert
            // This test verifies the current behavior, which does not throw when null is passed
            Assert.DoesNotThrow(() => new UserService(null));

            // Note: If you want to add null checking to your constructor, this test should be changed to:
            // Assert.Throws<ArgumentNullException>(() => new UserService(null));
        }

        [Test]
        public void Constructor_WithValidRepository_ShouldInitializeService()
        {
            // Arrange
            var repo = new Mock<IRepository<User>>();

            // Act
            var service = new UserService(repo.Object);

            // Assert - test that the service is properly initialized by calling a method
            Assert.DoesNotThrow(() => service.GetAll());
        }

        [Test]
        public async Task UserWithNavigationProperties_ShouldWorkCorrectly()
        {
            // Arrange
            string userId = "user1";
            var user = new User
            {
                Id = userId,
                UserName = "testuser",
                Name = "Test User",
                Shelves = new List<Shelf>
                {
                    new Shelf { Id = 1, Name = "Favorites", UserId = userId },
                    new Shelf { Id = 2, Name = "To Read", UserId = userId }
                },
                CurrentReads = new List<CurrentRead>
                {
                    new CurrentRead { Id = 1, BookId = 10, UserId = userId, CurrentPage = 50 }
                },
                Notes = new List<Notes>
                {
                    new Notes { Id = 1, Title = "Chapter 1 Notes", UserId = userId, BookChapter = 1 }
                }
            };

            _mockRepo.Setup(r => r.GetByIdUser(userId)).ReturnsAsync(user);

            // Act
            var result = await _userService.GetByIdUser(userId);

            // Assert
            Assert.That(result, Is.EqualTo(user));
            Assert.That(result.Shelves.Count, Is.EqualTo(2));
            Assert.That(result.CurrentReads.Count, Is.EqualTo(1));
            Assert.That(result.Notes.Count, Is.EqualTo(1));

            Assert.That(result.Shelves.Any(s => s.Name == "Favorites"), Is.True);
            Assert.That(result.Shelves.Any(s => s.Name == "To Read"), Is.True);
            Assert.That(result.CurrentReads.Any(cr => cr.BookId == 10 && cr.CurrentPage == 50), Is.True);
            Assert.That(result.Notes.Any(n => n.Title == "Chapter 1 Notes" && n.BookChapter == 1), Is.True);
        }

        [Test]
        public async Task UserWorkflow_AddFindUpdateDelete_ShouldWorkCorrectly()
        {
            // Arrange
            string userId = "user1";

            var user = new User
            {
                Id = userId,
                UserName = "testuser",
                Name = "Test User"
            };

            var updatedUser = new User
            {
                Id = userId,
                UserName = "testuser",
                Name = "Updated User Name"
            };

            _mockRepo.Setup(r => r.Add(It.IsAny<User>())).Returns(Task.CompletedTask);
            _mockRepo.Setup(r => r.Find(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(new List<User> { user });
            _mockRepo.Setup(r => r.Update(It.IsAny<User>())).Returns(Task.CompletedTask);
            _mockRepo.Setup(r => r.DeleteUser(It.IsAny<string>())).Returns(Task.CompletedTask);

            // Act & Assert - Full workflow
            // 1. Add a user
            await _userService.Add(user);
            _mockRepo.Verify(r => r.Add(user), Times.Once);

            // 2. Find the user
            var foundUsers = await _userService.Find(u => u.UserName == "testuser");
            Assert.That(foundUsers.Count, Is.EqualTo(1));
            Assert.That(foundUsers[0], Is.EqualTo(user));

            // 3. Update the user
            await _userService.Update(updatedUser);
            _mockRepo.Verify(r => r.Update(updatedUser), Times.Once);

            // 4. Delete the user using DeleteUser
            await _userService.DeleteUser(userId);
            _mockRepo.Verify(r => r.DeleteUser(userId), Times.Once);
        }
    }
}
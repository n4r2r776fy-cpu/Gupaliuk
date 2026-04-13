using System;
using Xunit;
using Moq;
using Lab31.Core;

namespace Lab31.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockRepo;
        private readonly Mock<IPasswordHasher> _mockHasher;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            // Ініціалізуємо Mock-об'єкти перед кожним тестом
            _mockRepo = new Mock<IUserRepository>();
            _mockHasher = new Mock<IPasswordHasher>();

            // Передаємо "фейкові" залежності (Mock.Object) у сервіс
            _userService = new UserService(_mockRepo.Object, _mockHasher.Object);
        }

        // --- ТЕСТИ РЕЄСТРАЦІЇ ---

        [Fact]
        public void RegisterUser_ValidData_HashesPasswordAndAddsUser()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetByEmail("test@test.com")).Returns((User)null);
            _mockHasher.Setup(hasher => hasher.HashPassword("password123")).Returns("hashed_pwd");

            // Act
            _userService.RegisterUser("test@test.com", "password123");

            // Assert
            _mockHasher.Verify(h => h.HashPassword("password123"), Times.Once); // Перевіряємо, чи викликався хешер
            _mockRepo.Verify(r => r.Add(It.Is<User>(u => u.Email == "test@test.com" && u.PasswordHash == "hashed_pwd")), Times.Once); // Перевіряємо, чи додався юзер
        }

        [Fact]
        public void RegisterUser_ExistingEmail_ThrowsInvalidOperationException()
        {
            // Arrange: налаштовуємо мок так, ніби юзер вже є в базі
            _mockRepo.Setup(repo => repo.GetByEmail("exist@test.com")).Returns(new User { Email = "exist@test.com" });

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _userService.RegisterUser("exist@test.com", "pass"));
            _mockRepo.Verify(r => r.Add(It.IsAny<User>()), Times.Never); // Переконуємось, що метод Add НЕ викликався
        }

        [Theory]
        [InlineData("", "pass")]
        [InlineData("test@test.com", "")]
        public void RegisterUser_EmptyInputs_ThrowsArgumentException(string email, string password)
        {
            Assert.Throws<ArgumentException>(() => _userService.RegisterUser(email, password));
        }

        // --- ТЕСТИ ЛОГІНУ ---

        [Fact]
        public void Login_ValidCredentials_ReturnsTrue()
        {
            // Arrange
            var user = new User { Email = "user@test.com", PasswordHash = "hashed_pwd" };
            _mockRepo.Setup(repo => repo.GetByEmail("user@test.com")).Returns(user);
            _mockHasher.Setup(hasher => hasher.VerifyPassword("correct_pass", "hashed_pwd")).Returns(true);

            // Act
            var result = _userService.Login("user@test.com", "correct_pass");

            // Assert
            Assert.True(result);
            _mockHasher.Verify(h => h.VerifyPassword("correct_pass", "hashed_pwd"), Times.Once);
        }

        [Fact]
        public void Login_InvalidPassword_ReturnsFalse()
        {
            // Arrange
            var user = new User { Email = "user@test.com", PasswordHash = "hashed_pwd" };
            _mockRepo.Setup(repo => repo.GetByEmail("user@test.com")).Returns(user);
            _mockHasher.Setup(hasher => hasher.VerifyPassword("wrong_pass", "hashed_pwd")).Returns(false);

            // Act
            var result = _userService.Login("user@test.com", "wrong_pass");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Login_UserNotFound_ReturnsFalse()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetByEmail("notfound@test.com")).Returns((User)null);

            // Act
            var result = _userService.Login("notfound@test.com", "pass");

            // Assert
            Assert.False(result);
            _mockHasher.Verify(h => h.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        // --- ТЕСТИ ЗМІНИ ПАРОЛЯ ---

        [Fact]
        public void ChangePassword_ValidUser_UpdatesPassword()
        {
            // Arrange
            var user = new User { Email = "user@test.com", PasswordHash = "old_hash" };
            _mockRepo.Setup(repo => repo.GetByEmail("user@test.com")).Returns(user);
            _mockHasher.Setup(hasher => hasher.HashPassword("new_pass")).Returns("new_hash");

            // Act
            _userService.ChangePassword("user@test.com", "new_pass");

            // Assert
            Assert.Equal("new_hash", user.PasswordHash);
            _mockRepo.Verify(r => r.Update(user), Times.Once);
        }

        [Fact]
        public void ChangePassword_UserNotFound_ThrowsInvalidOperationException()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetByEmail("ghost@test.com")).Returns((User)null);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _userService.ChangePassword("ghost@test.com", "new_pass"));
            _mockRepo.Verify(r => r.Update(It.IsAny<User>()), Times.Never);
        }

        // --- ТЕСТ КОНСТРУКТОРА ---

        [Fact]
        public void Constructor_NullDependencies_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new UserService(null, _mockHasher.Object));
            Assert.Throws<ArgumentNullException>(() => new UserService(_mockRepo.Object, null));
        }
    }
}
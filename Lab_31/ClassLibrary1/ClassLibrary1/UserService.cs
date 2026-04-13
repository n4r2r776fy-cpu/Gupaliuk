using System;

namespace Lab31.Core
{
    // Модель користувача
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }

    // Інтерфейс для роботи з базою даних (Залежність 1)
    public interface IUserRepository
    {
        User GetByEmail(string email);
        void Add(User user);
        void Update(User user);
    }

    // Інтерфейс для хешування паролів (Залежність 2)
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }

    // Основний сервіс з бізнес-логікою
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        // Dependency Injection через конструктор
        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        public void RegisterUser(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Email та пароль не можуть бути порожніми.");

            // Перевіряємо, чи є вже такий юзер
            if (_userRepository.GetByEmail(email) != null)
                throw new InvalidOperationException("Користувач з таким email вже існує.");

            var hashedPassword = _passwordHasher.HashPassword(password);
            var newUser = new User { Email = email, PasswordHash = hashedPassword };

            _userRepository.Add(newUser);
        }

        public bool Login(string email, string password)
        {
            var user = _userRepository.GetByEmail(email);
            if (user == null) return false;

            return _passwordHasher.VerifyPassword(password, user.PasswordHash);
        }

        public void ChangePassword(string email, string newPassword)
        {
            var user = _userRepository.GetByEmail(email);
            if (user == null)
                throw new InvalidOperationException("Користувача не знайдено.");

            user.PasswordHash = _passwordHasher.HashPassword(newPassword);
            _userRepository.Update(user);
        }
    }
}
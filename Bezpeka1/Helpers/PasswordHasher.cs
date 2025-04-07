using Bezpeka1.Helpers.Interfaces;

namespace Bezpeka1.Helpers
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Hash1(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password cannot be null or empty", nameof(password));

            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public string Hash(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password cannot be null or empty", nameof(password));

            // Конвертуємо пароль в число, наприклад, за допомогою кодування ASCII або іншої методики
            double passwordNumeric = password.Sum(c => (int)c);

            double a = 10;  // Додайте значення для a, яке ви хочете використовувати
            double x = passwordNumeric; // Тут використовуємо числове значення пароля

            // lg(a/x) - логарифм за основою 10
            double hashedValue = Math.Log10(a / x);

            return hashedValue.ToString();
        }

        public bool Verify1(string password, string hash)
        {
            if (string.IsNullOrEmpty(password))
                return false;

            if (string.IsNullOrEmpty(hash))
                return false;

            return BCrypt.Net.BCrypt.Verify(password, hash);
        }

        public bool Verify(string password, string hash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hash))
                return false;

            // Генеруємо новий хеш для введеного пароля
            string generatedHash = Hash(password);

            // Порівнюємо збережений хеш із згенерованим хешем
            return generatedHash == hash;
        }
    }
}

using Bezpeka1.Helpers.Interfaces;
using System.Globalization;

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

            string encrypted = CaesarEncrypt(password, 3);

            // Конвертуємо пароль в число, наприклад, за допомогою кодування ASCII або іншої методики
            double passwordNumeric = encrypted.Sum(c => (int)c);

            double a = 10;  // Додайте значення для a, яке ви хочете використовувати
            double x = passwordNumeric; // Тут використовуємо числове значення пароля

            // lg(a/x) - логарифм за основою 10
            double hashedValue = Math.Log10(a / x);

            return hashedValue.ToString(CultureInfo.InvariantCulture);
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

            string generatedHash = Hash(password);

            // Порівнюємо результат
            return generatedHash == hash;
        }

        private static string CaesarEncrypt(string input, int shift)
        {
            char[] buffer = input.ToCharArray();

            for (int i = 0; i < buffer.Length; i++)
            {
                char letter = buffer[i];
                if (char.IsLetter(letter))
                {
                    char d = char.IsUpper(letter) ? 'A' : 'a';
                    letter = (char)((((letter + shift) - d) % 26 + 26) % 26 + d);
                    buffer[i] = letter;
                }
            }

            return new string(buffer);
        }
    }
}

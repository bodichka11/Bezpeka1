namespace Bezpeka1.Helpers
{
    public static class PasswordValidator
    {
        public static bool Validate(string password)
        {
            return password.Length >= 8 &&
                   password.Any(char.IsUpper) &&
                   password.Any(char.IsDigit);
        }
    }
}

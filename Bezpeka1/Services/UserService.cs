using Bezpeka1.Data;
using Bezpeka1.Models.Enums;
using Bezpeka1.Models;
using Bezpeka1.Helpers;
using Bezpeka1.Helpers.Interfaces;
using Bezpeka1.Services.Interfaces;

namespace Bezpeka1.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(AppDbContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public User? GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public User? GetUserByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public User? ValidateUser(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username && !u.IsBlocked);
            if (user == null)
            {
                // Log user not found
                Console.WriteLine($"User {username} not found or blocked.");
                return null;
            }

            if (!_passwordHasher.Verify(password, user.PasswordHash))
            {
                // Log password mismatch
                Console.WriteLine($"Password for user {username} is incorrect.");
                return null;
            }

            return user;
        }

        public bool AddUser(string username, Role role = Role.User)
        {
            if (_context.Users.Any(u => u.Username == username))
                return false;

            var newUser = new User
            {
                Username = username,
                PasswordHash = "",
                Role = role,
                IsBlocked = false,
                PasswordRestrictionsEnabled = true
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();
            return true;
        }

        public bool BlockUser(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user == null) return false;

            user.IsBlocked = true;
            _context.SaveChanges();
            return true;
        }

        public bool UnblockUser(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user == null) return false;

            user.IsBlocked = false;
            _context.SaveChanges();
            return true;
        }

        public bool ChangePassword(int userId, string oldPassword, string newPassword)
        {
            var user = _context.Users.Find(userId);
            if (user == null) return false;

            if (!string.IsNullOrEmpty(user.PasswordHash) &&
                !_passwordHasher.Verify(oldPassword, user.PasswordHash))
            {
                return false;
            }

            if (string.IsNullOrEmpty(newPassword)) return false;

            if (!PasswordValidator.Validate(newPassword))
            {
                Console.WriteLine("New password must be at least 8 characters long, contain uppercase letters, digits, and arithmetic symbols.");
                return false;
            }

            user.PasswordHash = _passwordHasher.Hash(newPassword);
            _context.SaveChanges();
            return true;
        }

        public bool TogglePasswordRestrictions(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user == null) return false;

            user.PasswordRestrictionsEnabled = !user.PasswordRestrictionsEnabled;
            _context.SaveChanges();
            return true;
        }


        public void LogLoginLogout(string userId, string action, string userRole)
        {
            var logEntry = new LoginLogoutLog
            {
                UserId = userId,
                Action = action,
                Timestamp = DateTime.UtcNow,
                UserRole = userRole
            };

            _context.LoginLogoutLogs.Add(logEntry);
            _context.SaveChanges();
        }

        public void LogOperation(string userId, string action)
        {
            var logEntry = new OperationLog
            {
                UserId = userId,
                Action = action,
                Timestamp = DateTime.UtcNow
            };

            _context.OperationLogs.Add(logEntry);
            _context.SaveChanges();
        }

        public bool RegisterUser(string username, string password, Role role = Role.User)
        {
            if (_context.Users.Any(u => u.Username == username))
                return false;

            if (!PasswordValidator.Validate(password))
            {
                Console.WriteLine("Password does not meet the requirements.");
                return false;
            }

            var newUser = new User
            {
                Username = username,
                PasswordHash = _passwordHasher.Hash(password),
                Role = role,
                IsBlocked = false,
                PasswordRestrictionsEnabled = true,
                LastLogin = null
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();
            return true;
        }

        public List<LoginLogoutLog> GetAllLoginLogoutLogs()
        {
            return _context.LoginLogoutLogs.ToList();
        }

        public List<OperationLog> GetAllOperationLogs()
        {
            return _context.OperationLogs.ToList();
        }

        public bool ResetPassword(int userId, string newPassword)
        {
            var user = _context.Users.Find(userId);
            if (user == null) return false;

            if (!PasswordValidator.Validate(newPassword))
            {
                Console.WriteLine("New password does not meet the requirements.");
                return false;
            }

            user.PasswordHash = _passwordHasher.Hash(newPassword);
            _context.SaveChanges();
            return true;
        }
    }
}

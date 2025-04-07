using Bezpeka1.Models;
using Bezpeka1.Models.Enums;

namespace Bezpeka1.Services.Interfaces
{
    public interface IUserService
    {
        User? GetUserById(int id);

        User? GetUserByUsername(string username);

        List<User> GetAllUsers();

        User? ValidateUser(string username, string password);

        bool AddUser(string username, Role role = Role.User);

        bool BlockUser(int userId);

        bool UnblockUser(int userId);

        bool ChangePassword(int userId, string oldPassword, string newPassword);

        bool TogglePasswordRestrictions(int userId);

        bool RegisterUser(string username, string password, Role role = Role.User);

        bool ResetPassword(int userId, string newPassword);

        List<OperationLog> GetAllOperationLogs();

        List<LoginLogoutLog> GetAllLoginLogoutLogs();

        void LogLoginLogout(string userId, string action, string userRole);

        void LogOperation(string userId, string action);
    }
}

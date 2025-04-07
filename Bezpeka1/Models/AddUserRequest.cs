using Bezpeka1.Models.Enums;

namespace Bezpeka1.Models
{
    public class AddUserRequest
    {
        public string Username { get; set; } = string.Empty;
        public Role Role { get; set; } = Role.User;
    }
}

using Bezpeka1.Models.Enums;

namespace Bezpeka1.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public Role Role { get; set; }

        public bool IsBlocked { get; set; }

        public bool PasswordRestrictionsEnabled { get; set; }

        public DateTime? LastLogin { get; set; }
    }
}

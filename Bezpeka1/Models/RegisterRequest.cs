using Bezpeka1.Models.Enums;

namespace Bezpeka1.Models
{
    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }
}

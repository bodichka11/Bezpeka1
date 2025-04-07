namespace Bezpeka1.Models
{
    public class PasswordChangeRequest
    {
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}

namespace Bezpeka1.Models
{
    public class CaptchaValidationRequest
    {
        public int CaptchaId { get; set; }
        public string UserAnswer { get; set; } = string.Empty;
    }
}

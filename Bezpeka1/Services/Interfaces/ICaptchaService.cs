using Bezpeka1.Models;

namespace Bezpeka1.Services.Interfaces
{
    public interface ICaptchaService
    {
        Task<CaptchaImage> GetRandomCaptchaAsync();

        bool ValidateCaptcha(int captchaId, string userAnswer);

        Task<CaptchaImage> GenerateAndSaveCaptchaAsync();
    }
}

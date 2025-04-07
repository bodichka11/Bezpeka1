namespace Bezpeka1.Services.Interfaces
{
    public interface IReCaptchaService
    {
        Task<bool> VerifyReCaptchaAsync(string token);
    }
}

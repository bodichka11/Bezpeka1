using Bezpeka1.Models;
using Bezpeka1.Services.Interfaces;

namespace Bezpeka1.Services
{
    public class ReCaptchaService : IReCaptchaService
    {
        private readonly HttpClient _httpClient;
        private readonly string _secretKey;

        public ReCaptchaService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _secretKey = configuration["GoogleReCaptcha:SecretKey"];
        }

        public async Task<bool> VerifyReCaptchaAsync(string token)
        {
            var response = await _httpClient.PostAsJsonAsync("https://www.google.com/recaptcha/api/siteverify", new
            {
                secret = _secretKey,
                response = token
            });

            var result = await response.Content.ReadFromJsonAsync<ReCaptchaResponse>();
            return result?.Success ?? false;
        }
    }
}

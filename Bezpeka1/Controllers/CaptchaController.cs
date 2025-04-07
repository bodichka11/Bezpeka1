using Bezpeka1.Models;
using Bezpeka1.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Bezpeka1.Controllers
{
    [ApiController]
    [Route("api/captcha")]
    public class CaptchaController : ControllerBase
    {
        private readonly ICaptchaService _captchaService;
        private readonly IReCaptchaService _reCaptchaService;

        public CaptchaController(ICaptchaService captchaService, IReCaptchaService reCaptchaService)
        {
            _captchaService = captchaService;
            _reCaptchaService = reCaptchaService;
        }

        [HttpGet("random")]
        public async Task<IActionResult> GetRandomCaptcha()
        {
            var captcha = await _captchaService.GenerateAndSaveCaptchaAsync();

            return Ok(new
            {
                id = captcha.Id,
                imagePath = $"data:image/png;base64,{Convert.ToBase64String(captcha.ImageData)}"
            });
        }

        [HttpPost("validate")]
        public IActionResult ValidateCaptcha([FromBody] CaptchaValidationRequest request)
        {
            var isValid = _captchaService.ValidateCaptcha(request.CaptchaId, request.UserAnswer);
            return Ok(new { success = isValid, message = isValid ? "CAPTCHA validation succeeded." : "CAPTCHA validation failed." });
        }

        [HttpPost("recaptcha-validate")]
        public async Task<IActionResult> ValidateRecaptcha([FromBody] ReCaptchaValidationRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Token))
            {
                return BadRequest(new { success = false, message = "Invalid request. Token is required." });
            }

            bool isValid = await ValidateRecaptchaWithGoogle(request.Token);
            if (!isValid)
            {
                return BadRequest(new { success = false, message = "Invalid reCAPTCHA token." });
            }

            return Ok(new { success = true, message = "reCAPTCHA validated successfully!" });
        }

        private async Task<bool> ValidateRecaptchaWithGoogle(string token)
        {
            string googleRecaptchaUrl = "https://www.google.com/recaptcha/api/siteverify";
            string secretKey = "6LdyzAwrAAAAAOpJ6SHXfV8Izb8wBqaDjqpn7AH0"; // Замініть на ваш Secret Key

            var content = new FormUrlEncodedContent(new[]
            {
        new KeyValuePair<string, string>("secret", secretKey),
        new KeyValuePair<string, string>("response", token)
    });

            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.PostAsync(googleRecaptchaUrl, content);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        var jsonResponse = JObject.Parse(responseString);

                        return jsonResponse["success"]?.Value<bool>() ?? false;
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode}");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    return false;
                }
            }
        }
    }
}

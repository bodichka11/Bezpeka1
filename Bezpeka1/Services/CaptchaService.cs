using Bezpeka1.Data;
using Bezpeka1.Models;
using Bezpeka1.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Bezpeka1.Services
{
    public class CaptchaService : ICaptchaService
    {
        private readonly AppDbContext _context;

        public CaptchaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CaptchaImage> GetRandomCaptchaAsync()
        {
            var captchaImages = await _context.CaptchaImages.ToListAsync();
            var randomCaptcha = captchaImages.OrderBy(c => Guid.NewGuid()).FirstOrDefault();
            return randomCaptcha!;
        }

        public bool ValidateCaptcha(int captchaId, string userAnswer)
        {
            var captcha = _context.CaptchaImages.FirstOrDefault(c => c.Id == captchaId);

            return captcha!.Answer.Equals(userAnswer, StringComparison.OrdinalIgnoreCase);
        }

        public async Task<CaptchaImage> GenerateAndSaveCaptchaAsync()
{
    var (text, imageData) = await GenerateCaptchaAsync();

    var captchaImage = new CaptchaImage
    {
        Answer = text,
        ImageData = imageData,
        CreatedAt = DateTime.UtcNow
    };

    _context.CaptchaImages.Add(captchaImage);
    await _context.SaveChangesAsync();

    return captchaImage;
}

        public async Task<(string Text, byte[] ImageData)> GenerateCaptchaAsync()
        {
            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var captchaText = new string(Enumerable.Repeat(characters, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            using var bitmap = new Bitmap(200, 50);
            using var graphics = Graphics.FromImage(bitmap);

            // Заповнення фону
            graphics.Clear(Color.White);

            // Додавання тексту
            using var font = new System.Drawing.Font("Arial", 20, FontStyle.Bold);
            graphics.DrawString(captchaText, font, Brushes.Black, new PointF(10, 10));

            // Додавання шуму
            AddNoise(graphics, bitmap.Width, bitmap.Height);

            // Конвертація зображення в масив байтів
            using var memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, ImageFormat.Png);
            var imageData = memoryStream.ToArray();

            return (captchaText, imageData);
        }

        private void AddNoise(Graphics graphics, int width, int height)
        {
            var random = new Random();

            // Додавання випадкових ліній
            for (int i = 0; i < 5; i++)
            {
                graphics.DrawLine(
                    Pens.Gray,
                    random.Next(width),
                    random.Next(height),
                    random.Next(width),
                    random.Next(height)
                );
            }

            // Додавання випадкових точок
            for (int i = 0; i < 50; i++)
            {
                graphics.FillEllipse(
                    Brushes.Gray,
                    random.Next(width),
                    random.Next(height),
                    2,
                    2
                );
            }
        }
    }
}

namespace Bezpeka1.Models
{
    public class CaptchaImage
    {
        public int Id { get; set; }
        public string Answer { get; set; }
        public byte[] ImageData { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

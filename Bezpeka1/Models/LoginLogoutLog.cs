namespace Bezpeka1.Models
{
    public class LoginLogoutLog
    {
        public int Id { get; set; }
        public string UserId { get; set; } // Ідентифікатор користувача
        public string Action { get; set; } // "Login" або "Logout"
        public DateTime Timestamp { get; set; } // Дата та час події
        public string UserRole { get; set; } // Рівень доступу користувача
    }
}

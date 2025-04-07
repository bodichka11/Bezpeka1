namespace Bezpeka1.Models
{
    public class OperationLog
    {
        public int Id { get; set; }
        public string UserId { get; set; } // Ідентифікатор користувача
        public string Action { get; set; } // Опис дії (наприклад, "Change Password")
        public DateTime Timestamp { get; set; } // Дата та час виконання дії
    }
}

using Bezpeka1.Services.Interfaces;

namespace Bezpeka1.Helpers
{
    public class LogoutLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _scopeFactory;

        public LogoutLoggingMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
        {
            _next = next;
            _scopeFactory = scopeFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == 401 || context.Response.StatusCode == 403)
            {
                var userId = context.User?.Identity?.Name;
                if (!string.IsNullOrEmpty(userId))
                {
                    // Створюємо новий scope для виклику UserService
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                        userService.LogLoginLogout(userId, "Logout", "Unknown");
                    }
                }
            }
        }
    }
}

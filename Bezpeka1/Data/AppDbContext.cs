using Bezpeka1.Helpers;
using Bezpeka1.Models;
using Microsoft.EntityFrameworkCore;

namespace Bezpeka1.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<LoginLogoutLog> LoginLogoutLogs { get; set; }
        public DbSet<OperationLog> OperationLogs { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var passwordHasher = new PasswordHasher();

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin1",
                    PasswordHash = passwordHasher.Hash("admin@123"),
                    Role = Models.Enums.Role.Admin,
                    IsBlocked = false,
                    PasswordRestrictionsEnabled = true
                }
            );
        }

        public async Task SeedAdminUser(AppDbContext context)
        {
            var passwordHasher = new PasswordHasher();
            var predefinedAdmins = new List<User>
            {
                new() { Username = "admin", PasswordHash = passwordHasher.Hash("admin123"), Role = Models.Enums.Role.Admin, IsBlocked = false, PasswordRestrictionsEnabled = true },
            };

            foreach (var user in predefinedAdmins)
            {
                // Перевіряємо, чи вже існує такий користувач в базі даних
                var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
                if (existingUser == null)  // Додаємо користувача тільки якщо його ще немає
                {
                    await context.Users.AddAsync(user);
                }
            }

            await context.SaveChangesAsync();
        }

    }
}

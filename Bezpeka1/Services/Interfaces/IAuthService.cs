using Bezpeka1.Models;

namespace Bezpeka1.Services.Interfaces
{
    public interface IAuthService
    {
        string GenerateJwtToken(User user);
    }
}

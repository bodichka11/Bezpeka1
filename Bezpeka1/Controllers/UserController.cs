using Bezpeka1.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bezpeka1.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("profile")]
        [Authorize(Roles = "User,Admin")]
        public IActionResult GetProfile()
        {
            var username = User.Identity?.Name;
            var user = _userService.GetUserByUsername(username!);

            if (user == null) return NotFound();

            return Ok(new
            {
                user.Username,
                user.Role,
                user.IsBlocked,
                user.PasswordRestrictionsEnabled
            });
        }

        [HttpGet("last-login")]
        [Authorize(Roles ="Manager,Moderator")]
        public IActionResult GetLastLogin()
        {
            var username = User.Identity?.Name;
            var user = _userService.GetUserByUsername(username!);

            if (user == null) return NotFound();

            return Ok(new
            {
                user.Username,
                user.LastLogin
            });
        }
    }
}

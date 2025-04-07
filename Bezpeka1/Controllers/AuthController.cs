using Bezpeka1.Models;
using Bezpeka1.Services;
using Bezpeka1.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bezpeka1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public AuthController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] AuthRequest request)
        {
            var user = _userService.ValidateUser(request.Username, request.Password);
            if (user == null) return Unauthorized("Invalid username or password");

            var token = _authService.GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

        [HttpPost("change-password")]
        [Authorize]
        public IActionResult ChangePassword([FromBody] PasswordChangeRequest request)
        {
            var username = User.Identity?.Name;
            var user = _userService.GetUserByUsername(username);

            if (user == null) return NotFound("User not found");

            var success = _userService.ChangePassword(user.Id, request.OldPassword, request.NewPassword);
            return success ? Ok() : BadRequest("Invalid password or restrictions not met");
        }
    }
}

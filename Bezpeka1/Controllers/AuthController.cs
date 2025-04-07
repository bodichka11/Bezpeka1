using Bezpeka1.Models;
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

            _userService.LogLoginLogout(user.Id.ToString(), "Login", user.Role.ToString());

            var token = _authService.GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] Models.RegisterRequest request)
        {
            if (request == null || request.Request == null)
            {
                return BadRequest("Invalid request payload.");
            }

            Console.WriteLine($"Received registration request: {request.Request.Username}, {request.Request.Password}, {request.Request.Role}");

            var success = _userService.RegisterUser(request.Request.Username, request.Request.Password, request.Request.Role);
            return success ? Ok() : BadRequest("Registration failed");
        }

        [HttpPost("change-password")]
        [Authorize]
        public IActionResult ChangePassword([FromBody] PasswordChangeRequest request)
        {
            var username = User.Identity?.Name;
            var user = _userService.GetUserByUsername(username);

            if (user == null) return NotFound("User not found");

            var success = _userService.ChangePassword(user.Id, request.OldPassword, request.NewPassword);
            if (success)
            {
                // Логування дії
                _userService.LogOperation(user.Id.ToString(), "Change Password");
                return Ok();
            }

            return BadRequest("Invalid password or restrictions not met");
        }
    }
}

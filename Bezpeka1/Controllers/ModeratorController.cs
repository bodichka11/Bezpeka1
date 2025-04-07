using Bezpeka1.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bezpeka1.Controllers
{
    [ApiController]
    [Route("api/moderator")]
    [Authorize(Roles = "Moderator")]
    public class ModeratorController : ControllerBase
    {
        private readonly IUserService _userService;

        public ModeratorController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("operation-logs")]
        public IActionResult GetAllOperationLogs()
        {
            var logs = _userService.GetAllOperationLogs();
            return Ok(logs);
        }

        [HttpPost("reset-password/{userId}")]
        public IActionResult ResetPassword(int userId, [FromBody] string newPassword)
        {
            var success = _userService.ResetPassword(userId, newPassword);
            return success ? Ok() : BadRequest("Failed to reset password");
        }
    }
}

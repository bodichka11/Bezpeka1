using Bezpeka1.Models;
using Bezpeka1.Services;
using Bezpeka1.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bezpeka1.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("users")]
        public IActionResult GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpPost("add-user")]
        public IActionResult AddUser([FromBody] AddUserRequest request)
        {
            var success = _userService.AddUser(request.Username, request.Role);
            return success ? Ok() : BadRequest("Username already exists");
        }

        [HttpPost("block/{userId}")]
        public IActionResult BlockUser(int userId)
        {
            var success = _userService.BlockUser(userId);
            return success ? Ok() : BadRequest("User not found");
        }

        [HttpPost("unblock/{userId}")]
        public IActionResult UnblockUser(int userId)
        {
            var success = _userService.UnblockUser(userId);
            return success ? Ok() : BadRequest("User not found");
        }

        [HttpPost("toggle-restrictions/{userId}")]
        public IActionResult TogglePasswordRestrictions(int userId)
        {
            var success = _userService.TogglePasswordRestrictions(userId);
            return success ? Ok() : BadRequest("User not found");
        }
    }
}

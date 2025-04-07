using Bezpeka1.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bezpeka1.Controllers
{
    [ApiController]
    [Route("api/manager")]
    [Authorize(Roles = "Manager")]
    public class ManagerController : ControllerBase
    {
        private readonly IUserService _userService;

        public ManagerController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("login-logout-logs")]
        public IActionResult GetAllLoginLogoutLogs()
        {
            var logs = _userService.GetAllLoginLogoutLogs();
            return Ok(logs);
        }
    }
}

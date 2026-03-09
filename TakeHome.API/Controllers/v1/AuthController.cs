
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TakeHome.API.Dtos;
using TakeHome.API.Interface;

namespace TakeHome.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var user = await _authService.RegisterAsync(request.UserName, request.Password);
                return CreatedAtAction(nameof(GetUser), new { username = user.UserName }, new
                {
                    user.Id,
                    user.UserName
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var token = await _authService.LoginAsync(request.UserName, request.Password);

                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpGet("{username}")]
        [MapToApiVersion("1.0")]
        [Authorize]
        public async Task<IActionResult> GetUser(string username)
        {
            return Ok(new { message = $"User info for {username}" });
        }
    }
}

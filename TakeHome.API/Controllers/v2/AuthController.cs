
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TakeHome.API.Dtos;
using TakeHome.API.Interface;

namespace TakeHome.API.Controllers.v2
{
    [Route("api/v2/[controller]")]
    [ApiVersion("2.0")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [MapToApiVersion("2.0")]
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
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> LoginV2([FromBody] LoginRequest request)
        {
            try
            {
                var token = await _authService.LoginAsync(request.UserName, request.Password);

                return Ok(new { token, version = 2 });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpGet("{username}")]
        [MapToApiVersion("2.0")]    
        [Authorize]
        public async Task<IActionResult> GetUser(string username)
        {
            return Ok(new { message = $"User info for {username}" });
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TakeHome.API.Interface;
using TakeHome.API.Models;

namespace TakeHome.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext appDbContext, IConfiguration config)
        {
            _appDbContext = appDbContext;
            _config = config;
        }
        public async Task<string> LoginAsync(string username, string password)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null)
            {
                Log.Warning("Authentication failed for user: {Email}", username);
                throw new UnauthorizedAccessException();
            }
            ;

            var hash = HashPassword(password, user.Salt);
            if (hash != user.PasswordHash)
            {
                Log.Warning("Invalid password", username);
                throw new Exception("Invalid username or password");
            } 

            return GenerateJwtTokenAsync(user);
        }

        public async Task<User> RegisterAsync(string username, string password)
        {


            if (await _appDbContext.Users.AnyAsync(u => u.UserName == username))
                throw new Exception("Username already exists");

            var salt = GenerateSalt();
            var hash = HashPassword(password, salt);

            var user = new User
            {
                UserName = username,
                PasswordHash = hash,
                Salt = salt
            };

            _appDbContext.Users.Add(user);
            await _appDbContext.SaveChangesAsync();

            return user;
        }

        private string GenerateJwtTokenAsync(User user)
        {
            var claims = new[]
            {   
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_config["Jwt:ExpiryMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateSalt()
        {
            var bytes = new byte[16];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        private string HashPassword(string password, string salt)
        {
            using var sha256 = SHA256.Create();
            var combined = password + salt;
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combined));
            return Convert.ToBase64String(bytes);
        }
    }
}

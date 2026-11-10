using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthService.Models;
using AuthService.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Services
{
    public interface IAuthService
    {
        Task<string?> AuthenticateAsync(string username, string password);
        Task<User?> RegisterAsync(string username, string email, string password, string role);
        Task<User?> GetUserByUsernameAsync(string username);
    }

    public class AuthenticationService : IAuthService
    {
          private readonly AuthDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(
            AuthDbContext context,
            IConfiguration configuration,
            ILogger<AuthenticationService> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }
        
        public async Task<User?> RegisterAsync(string username, string email, string password, string role)
        {
             try
            {
                // Check if user already exists
                if (await _context.Users.AnyAsync(u => u.Username == username || u.Email == email))
                {
                    return null;
                }

                var user = new User
                {
                    Username = username,
                    Email = email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                    Role = role,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration");
                return null;
            }
        
            
        }

        public async Task<string?> AuthenticateAsync(string username, string password)
        {
            return "";
        }
          public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return (new User());
        }

        
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using RealEstateListingAPI.Contexts;
using RealEstateListingAPI.Models;

namespace RealEstateListingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly comp584DbContext _context;
        private readonly IConfiguration _configuration;

        public UsersController(comp584DbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserLoginDto loginDto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == loginDto.Email);

            if (user != null && BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                var token = GenerateJwtToken(user); // Generate JWT token
                return Ok(new { token = token, message = "Authentication successful" });
            }

            return Unauthorized(new { message = "Authentication failed" }); // Authentication failed
        }


        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                    // You can add more claims if needed
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegistrationDto registrationDto)
        {
            var userExists = _context.Users.Any(u => u.Email == registrationDto.Email);
            if (userExists)
            {
                return BadRequest("User already exists.");
            }

            var user = new User
            {
                Name = "name", // Default name
                Email = registrationDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registrationDto.Password),
                Role = "Buyer", // Default role
                                // Set other necessary properties, if any
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(new { message = "Registration successful" });
        }


    }

    public class UserRegistrationDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        // Add other registration fields as necessary
    }

    public class UserLoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }


}

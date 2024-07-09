using static BCrypt.Net.BCrypt;
using MyNotes.Contracts;
using Microsoft.AspNetCore.Mvc;
using MyNotes.DataAccess;
using MyNotes.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyNotes.Controllers
{
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly NotesDbContext _dbContext;
        private readonly IConfiguration _config;

        public AuthController(NotesDbContext dbContext, IConfiguration config)
        {
            _dbContext = dbContext;
            _config = config;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] AuthRequest request)
        {
           var user = _dbContext.Users.FirstOrDefault(user => user.Email == request.Email);
            if (user != null) {
                bool validate = Verify(request.Password, user.Password);
                if (validate)
                {
                    var claims = new[] { new Claim(ClaimTypes.UserData, user.Id.ToString()) };

                    string jwtIssuer = _config.GetSection("Jwt:Issuer").Get<string>() ?? "";
                    string jwtKey = _config.GetSection("Jwt:Key").Get<string>() ?? "";

                    var jwt = new JwtSecurityToken(
                         issuer: jwtIssuer,
                         audience: jwtIssuer,
                         claims,
                         expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(20)),
                         signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)), SecurityAlgorithms.HmacSha256)
                    );

                    string token = new JwtSecurityTokenHandler().WriteToken(jwt);
                    return new JsonResult(token);
                }
                return BadRequest(new { message = "Incorect request" });
            }
            return BadRequest(new { message = "Incorect request" });
        }


        [HttpPost("registration")]
        public IActionResult Registration([FromBody] AuthRequest request) {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Incorect request" });
            }

            var user = _dbContext.Users.FirstOrDefault(opt => opt.Email == request.Email);

            if (user == null)
            {
                string passwordHash = HashPassword(request.Password, 15);
            
                Users newUser = new() { Email = request.Email, Password = passwordHash };

                _dbContext.Users.Add(newUser);
                _dbContext.SaveChanges();

                return new JsonResult(request);
            }
            return BadRequest(new { message = "User already exists" });

        }

    }
}


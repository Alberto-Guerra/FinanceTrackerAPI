using FinanceTrackerAPI.Data;
using FinanceTrackerAPI.DTOs;
using FinanceTrackerAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FinanceTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
     

        public AuthController(DataContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }


        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDTO userDTO)
        {

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userDTO.Username);

            if (user != null)
            {
                return Unauthorized("User is taken");
            }

            CreatePasswordHash(userDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user = new User();
            user.Username = userDTO.Username;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;


            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenDTO>> Login(UserDTO userDTO)
        {
            //i want to get the user with the dto username, then if not found return bad request
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userDTO.Username);

            if (user == null)
            {
                  return Unauthorized("User not found");
            }


            if(!VerifyPasswordHash(userDTO.Password, user.PasswordHash, user.PasswordSalt))
            {
                return Unauthorized("Wrong password");
            }

            string token = CreateJWTToken(user);

            return Ok(new TokenDTO(token));
            

        }

        private string CreateJWTToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                               claims: claims,
                               expires: DateTime.Now.AddDays(1),
                               signingCredentials: cred
                               );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);     

            return jwt;
            
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();

            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt) 
        {
            using var hmac = new HMACSHA512(passwordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            return computedHash.SequenceEqual(passwordHash);

        }

    }
}

using System.Threading.Tasks;
using DatingApp.Data;
using Microsoft.AspNetCore.Mvc;
using DatingApp.Models;
using DatingApp.Dtos;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace DatingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _conf;
        public AuthController(IAuthRepository repo,IConfiguration conf)
        {
            _repo = repo;
            _conf = conf;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();
            if(!await _repo.UserExists(userForRegisterDto.Username))
                return BadRequest("Username is Already Exist");
            var userToCreate = new User{
                Username = userForRegisterDto.Username
            };
            var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password );
            return StatusCode(201);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDtos userForLoginDtos)
        {
            var user = await _repo.Login(userForLoginDtos.username,userForLoginDtos.password);
            if(user == null)
                return Unauthorized();
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,userForLoginDtos.username)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8
                            .GetBytes(_conf.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new{
                token = tokenHandler.WriteToken(token)
            });
        }
    }
}
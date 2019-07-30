using System.Threading.Tasks;
using DatingApp.Data;
using Microsoft.AspNetCore.Mvc;
using DatingApp.Models;
using DatingApp.Dtos;

namespace DatingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        public AuthController(IAuthRepository repo)
        {
            _repo = repo;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            if(!await _repo.UserExists(userForRegisterDto.Username))
                return BadRequest("Username is Already Exist");
            var userToCreate = new User{
                Username = userForRegisterDto.Username
            };
            var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password );
            return StatusCode(201);
        }
        [HttpGet("login")]
        public async Task<IActionResult> Login(string username,string password)
        {
            var user = await _repo.Login(username,password);
            if(user == null)
                return BadRequest();
            return Ok("User found");
        }
    }
}
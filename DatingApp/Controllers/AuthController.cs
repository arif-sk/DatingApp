using System.Threading.Tasks;
using DatingApp.Data;
using Microsoft.AspNetCore.Mvc;
using DatingApp.Models;

namespace DatingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthRepository _repo;
        public AuthController(AuthRepository repo)
        {
            _repo = repo;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(string username, string password)
        {
            username = username.ToString();
            if(await _repo.UserExists(username))
                return BadRequest("Username is Already Exist");
            var userToCreate = new User{
                Username = username
            };
            var createdUser = await _repo.Register(userToCreate, password);
            return StatusCode(201);
        }
    }
}
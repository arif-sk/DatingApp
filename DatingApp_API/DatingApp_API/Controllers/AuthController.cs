﻿using System.Threading.Tasks;
using DatingApp_API.Data;
using Microsoft.AspNetCore.Mvc;
using DatingApp_API.Models;
using DatingApp_API.Dtos;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using AutoMapper;

namespace DatingApp_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _conf;
        private IMapper _mapper;
        public AuthController(IAuthRepository repo, IConfiguration conf,IMapper mapper)
        {
            _repo = repo;
            _conf = conf;
            _mapper = mapper;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();
            if (!await _repo.UserExists(userForRegisterDto.Username))
                return BadRequest("Username is Already Exist");
            var userToCreate = new User
            {
                Username = userForRegisterDto.Username
            };
            var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password);
            return StatusCode(201);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDtos)
        {
            var userFromRepo = await _repo.Login(userForLoginDtos.username, userForLoginDtos.password);
            if (userFromRepo == null)
                return Unauthorized();
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name,userForLoginDtos.username)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8
                            .GetBytes(_conf.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var user = _mapper.Map<UserForListDto>(userFromRepo);
            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                user
            });
        }
    }
}
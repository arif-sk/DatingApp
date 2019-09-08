using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp_API.Data;
using DatingApp_API.Dtos;
using DatingApp_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository _datingRepository;
        private readonly IMapper _mapper;
        public UsersController(IDatingRepository datingRepository,IMapper mapper)
        {
            _datingRepository = datingRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IEnumerable<UserForDetailedDto>> Get()
        {
            var users = await _datingRepository.GetAllUsers();
            var usersToReturn = _mapper.Map<IEnumerable<UserForDetailedDto>>(users);
            return usersToReturn;
        }
        [HttpGet("{id}")]
        public async Task<UserForDetailedDto> Get(int id)
        {
            var user = await _datingRepository.GetUser(id);
            var userToReturn = _mapper.Map<UserForDetailedDto>(user);
            return userToReturn;
        }
    }
}
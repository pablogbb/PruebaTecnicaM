using Application.Dtos;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PruebaTecnica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost, Route("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserLoginDto userLoginDto)
        {
            var user = await _userService.GetUserByEmailAndPassword(userLoginDto);
            if(user == null)
            {
                throw new UnauthorizedAccessException($"Email o password inválidos");
            }

            return Ok();
        }

        [HttpPost, Route("Create")]
        public async Task<IActionResult> CreateUserAsync([FromBody] UserCreateDto userCreateDto)
        {
            await _userService.CreateUser(userCreateDto);
            return Ok();
        }
    }
}

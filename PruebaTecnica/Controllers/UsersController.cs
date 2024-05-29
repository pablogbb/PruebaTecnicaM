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
        public async Task<IActionResult> LoginAsync([FromBody] UserLoginDto userLoginRequest)
        {
            var user = await _userService.GetUserByEmailAndPassword(userLoginRequest.Email, userLoginRequest.Password);
            if(user == null)
            {
                throw new UnauthorizedAccessException($"Email o password inválidos");
            }

            return Ok();
        }
    }
}

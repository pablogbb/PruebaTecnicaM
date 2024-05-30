using Application.Dtos;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PruebaTecnica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly UserService _userService;

        public UsersController(UserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost, Route("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserLoginDto userLoginDto)
        {
            var user = await _userService.GetUserByEmailAndPassword(userLoginDto);
            if(user == null)
            {
                throw new UnauthorizedAccessException($"Email o password inválidos");
            }

            string secretKey = _configuration["Jwt:SecretKey"];
            string issuer = _configuration["Jwt:Issuer"];
            string sudience = _configuration["Jwt:Audience"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("id", user.Id.ToString()),
            };

            var tokenConfig = new JwtSecurityToken(
                issuer: issuer,
                audience: sudience,
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );

            var organizations = _userService.GetOrganizationsByUser(user.Id);

            var token = new JwtSecurityTokenHandler().WriteToken(tokenConfig);

            var tenants = organizations.Select(x => new { slugTenant = x.SlugTenant }).ToList();

            return Ok(new { accessToken = token, tenants  });
        }

        [HttpPost, Route("Create")]
        public async Task<IActionResult> CreateUserAsync([FromBody] UserCreateDto userCreateDto)
        {
            await _userService.CreateUser(userCreateDto);
            return Ok();
        }

        [HttpPost, Route("AssignOrganization")]
        public IActionResult AssignOrganization([FromBody] UserCreateDto userCreateDto)
        {
            _userService.AssingUserToOrg(userCreateDto.Id ?? 0, userCreateDto.OrganizationId);
            return Ok();
        }
    }
}

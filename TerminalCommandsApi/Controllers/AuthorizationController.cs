using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TerminalCommandsApi.Configurations;
using TerminalCommandsApi.Dto.Request;
using TerminalCommandsApi.Dto.Response;

namespace TerminalCommandsApi.Controllers
{
    [Route("api/[controller]")] // api/authManagement
    [ApiController]
    public class AuthorizationController : ControllerBase
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtConfig _jwtConfig;

        public AuthorizationController(
            UserManager<IdentityUser> userManager,
            IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegistrationDto user)
        {
            var existingUser = await _userManager.FindByEmailAsync(user.Email);

            if (existingUser != null)
            {
                return BadRequest(new {message = "Email already in use"});
            }

            var newUser = new IdentityUser {Email = user.Email, UserName = user.UserName};
            var isCreated = await _userManager.CreateAsync(newUser, user.Password);
            if (!isCreated.Succeeded) return BadRequest(new {message = $"Failed to create user {isCreated.Errors.First().Description} "});
            var jwtToken = GenerateJwtToken(newUser);

            return Ok(new SuccessDto()
            {
                Status = true,
                Token = jwtToken
            });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var existingUser = await _userManager.FindByEmailAsync(loginDto.Email);

            if (existingUser == null)
            {
                return BadRequest(new {message = "Invalid login request user doesn't exist"});
            }

            var isCorrect = await _userManager.CheckPasswordAsync(existingUser, loginDto.Password);

            if (!isCorrect)
            {
                return BadRequest(new {message = "Invalid user password"});
            }

            var jwtToken = GenerateJwtToken(existingUser);

            return Ok(new SuccessDto()
            {
                Status = true,
                Token = jwtToken
            });
        }

        private string GenerateJwtToken(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }
    }
}
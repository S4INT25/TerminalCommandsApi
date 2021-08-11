using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TerminalCommandsApi.Domain.Dto.Request;
using TerminalCommandsApi.Domain.Interfaces;


namespace TerminalCommandsApi.Controllers
{
    [Route("api/[controller]")] // api/authManagement
    [ApiController]
    public class AuthorizationController : ControllerBase
    {

        private readonly IAuthService _authService;

        public AuthorizationController(
            IAuthService service)
        {
            _authService = service;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegistrationDto user)
        {
            var result = await _authService.RegisterAsync(user);

            return Ok(result);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var result = await _authService.LoginAsync(loginRequest);
            return Ok(result);
        }


        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> Login([FromBody] RefreshTokenRequest tokenRequest)
        {
            var result = await _authService.RefreshTokenAsync(tokenRequest);
            return Ok(result);
        }


    }
}
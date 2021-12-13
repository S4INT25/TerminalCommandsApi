using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TerminalCommandsApi.Domain.Dto.Request;
using TerminalCommandsApi.Domain.Interfaces;
using TerminalCommandsApi.Hubs;


namespace TerminalCommandsApi.Controllers
{
    [Route("api/[controller]")] // api/authManagement
    [ApiController]
    public class AuthorizationController : ControllerBase
    {

        private readonly IHubContext<DataBaseMessageHub, IDataBaseHub> _hubContext;
        private readonly IAuthService _authService;

        public AuthorizationController(
            IAuthService service, IHubContext<DataBaseMessageHub, IDataBaseHub> hubContext)
        {
            _authService = service;
            _hubContext = hubContext;
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
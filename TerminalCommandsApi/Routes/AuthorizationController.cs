using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TerminalCommandsApi.Domain.Dto.Request;
using TerminalCommandsApi.Domain.Interfaces;

// namespace TerminalCommandsApi.Routes
// {
//     [Route("api/[controller]")] // api/authManagement
//     [ApiController]
//     public class AuthorizationController : ControllerBase
//     {
//
//         private readonly IHubContext<DataBaseMessageHub, IDataBaseHub> _hubContext;
//         private readonly IAuthService _authService;
//
//         public AuthorizationController(
//             IAuthService service, IHubContext<DataBaseMessageHub, IDataBaseHub> hubContext)
//         {
//             _authService = service;
//             _hubContext = hubContext;
//         }
//
//         [HttpPost]
//         [Route("Register")]
//         public async Task<IActionResult> Register([FromBody] RegistrationDto user)
//         {
//             var result = await _authService.RegisterAsync(user);
//
//             return Ok(result);
//         }
//
//         [HttpPost]
//         [Route("Login")]
//         public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
//         {
//             var result = await _authService.LoginAsync(loginRequest);
//             return Ok(result);
//         }
//
//
//         [HttpPost]
//         [Route("RefreshToken")]
//         public async Task<IActionResult> Login([FromBody] RefreshTokenRequest tokenRequest)
//         {
//             var result = await _authService.RefreshTokenAsync(tokenRequest);
//             return Ok(result);
//         }
//
//
//     }
// }


namespace TerminalCommandsApi.Routes;

public static class AuthorizationRoutes
{

    public static RouteGroupBuilder MapAuthEndpoints(this RouteGroupBuilder app)
    {
        app.MapPost("register", async (RegistrationDto user, IAuthService authService) =>
        {
            var result = await authService.RegisterAsync(user);

            return Results.Ok(result);
        }).Produces<ActionResult>();

        app.MapPost("login", async (LoginRequest user, IAuthService authService) =>
        {
            var result = await authService.LoginAsync(user);
            return Results.Ok(result);
        }).Produces<ActionResult>();
        
        app.MapPost("refresh-token", async (RefreshTokenRequest user, IAuthService authService) =>
        {
            var result = await authService.RefreshTokenAsync(user);
            return Results.Ok(result);
        }).Produces<ActionResult>();

        return app;
    }

}
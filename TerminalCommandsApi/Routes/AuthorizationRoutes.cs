using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TerminalCommandsApi.Domain.Dto.Request;
using TerminalCommandsApi.Domain.Interfaces;

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
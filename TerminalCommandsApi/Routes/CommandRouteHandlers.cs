using System.Linq;
using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TerminalCommandsApi.Domain.Dto.Request;
using TerminalCommandsApi.Domain.Interfaces;
using TerminalCommandsApi.Domain.Models;
using TerminalCommandsApi.Extensions;


namespace TerminalCommandsApi.Routes;

public static class CommandRouteHandlers
{

    public static RouteGroupBuilder MapCommandRoutes(this RouteGroupBuilder group)
    {
        group.MapGet("", async (ICommanderService commanderRepo) =>
        {
            var commands = await commanderRepo.GetCommands();
            var commandsDto = commands.Select(x => x.ToReadDto()).ToList();

            return Results.Ok(commandsDto);
        }).WithDescription("Get all commands");


        group.MapGet("{id:int}", async (int id, ICommanderService commanderService) =>
        {
            var command = await commanderService.GetCommandById(id);

            return Results.Ok(command.ToReadDto());
        }).WithDescription("Get a command by id");

        group.MapPost("", (CommandCreateDto commandCreateDto, HttpContext ctx, ICommanderService commanderService) =>
        {
            var command = new Command
            {
                Id = 0,
                Name = commandCreateDto.Name,
                Description = commandCreateDto.Description,
                Platform = commandCreateDto.Platform,
                CreatedById = ctx.GetUserId()
            };

            commanderService.CreateCommand(command);

            return !commanderService.SaveChanges() ? Results.BadRequest() : Results.Created($"api/{command.Id}", command.ToReadDto());
        }).WithDescription("Create a new command");

        group.MapPut("{id:int}", async (int id, CommandUpdateDto commandUpdateDto, ICommanderService commanderService) =>
        {
            var command = await commanderService.GetCommandById(id);

            commandUpdateDto.Adapt(command);

            commanderService.SaveChanges();
            return Results.Ok("Command updated");
        }).WithDescription("Update a command");


        group.MapDelete("{id:int}", async (int id, ICommanderService commanderService) =>
        {
            var command = await commanderService.GetCommandById(id);

            commanderService.DeleteCommand(command);
            commanderService.SaveChanges();
            return Results.Ok("Command deleted");
        }).WithDescription("Delete a command");

        return group;
    }

}
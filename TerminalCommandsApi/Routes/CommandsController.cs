using System.Collections.Generic;
using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TerminalCommandsApi.Domain.Dto.Request;
using TerminalCommandsApi.Domain.Dto.Response;
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
            return commands.Count.Equals(0) ? Results.NoContent() : Results.Ok(commands.Adapt<ICollection<CommandReadDto>>());
        }).WithDescription("Get all commands");


        group.MapGet("{id:int}", async (int id, ICommanderService commanderService) =>
        {
            var command = await commanderService.GetCommandById(id);

            return Results.Ok(command.Adapt<CommandReadDto>());
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

            return !commanderService.SaveChanges() ? Results.BadRequest() : Results.Created($"api/{command.Id}", command.Adapt<CommandReadDto>());
        }).WithDescription("Create a new command");

        group.MapPut("{id:int}", async (int id, CommandUpdateDto commandUpdateDto, ICommanderService commanderService) =>
        {
            var command = await commanderService.GetCommandById(id);

            commandUpdateDto.Adapt(command);

            commanderService.SaveChanges();
            return Results.NoContent();
        }).WithDescription("Update a command");


        group.MapDelete("{id:int}", async (int id, ICommanderService commanderService) =>
        {
            var command = await commanderService.GetCommandById(id);

            commanderService.DeleteCommand(command);
            commanderService.SaveChanges();
            return Results.NoContent();
        }).WithDescription("Delete a command");

        return group;
    }

}
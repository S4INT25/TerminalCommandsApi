using System.Collections.Generic;
using AutoMapper;
using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TerminalCommandsApi.Domain.Dto.Request;
using TerminalCommandsApi.Domain.Dto.Response;
using TerminalCommandsApi.Domain.Interfaces;
using TerminalCommandsApi.Domain.Models;


namespace TerminalCommandsApi.Routes;

public static class CommandRouteHandlers
{

    public static RouteGroupBuilder MapCommandRoutes(this RouteGroupBuilder group)
    {
        group.MapGet("", async (ICommanderService commanderRepo, IMapper mapper) =>
        {
            var commands = await commanderRepo.GetCommands();
            return commands.Count.Equals(0) ? Results.NoContent() : Results.Ok(mapper.Map<IEnumerable<CommandReadDto>>(commands));
        });


        group.MapGet("{id:int}", async (int id, ICommanderService commanderService, IMapper mapper) =>
        {
            var command = await commanderService.GetCommandById(id);

            return command == null ? Results.NoContent() : Results.Ok(mapper.Map<CommandReadDto>(command));
        });

        group.MapPost("", (CommandCreateDto commandCreateDto, ICommanderService commanderService, IMapper mapper) =>
        {
            var commandModel = mapper.Map<Command>(commandCreateDto);

            commanderService.CreateCommand(commandModel);

            return !commanderService.SaveChanges() ? Results.BadRequest() : Results.Created($"api/{commandModel.Id}", mapper.Map<CommandReadDto>(commandModel));
        });

        group.MapPut("{id:int}", async (int id, CommandUpdateDto commandUpdateDto, ICommanderService commanderService) =>
        {
            var command = await commanderService.GetCommandById(id);
            if (command == null)
            {
                return Results.NoContent();
            }

            commandUpdateDto.Adapt(command);

            commanderService.SaveChanges();
            return Results.NoContent();
        });


        group.MapDelete("{id:int}", async (int id, ICommanderService commanderService) =>
        {
            var command = await commanderService.GetCommandById(id);
            if (command == null)
            {
                return Results.NoContent();
            }

            commanderService.DeleteCommand(command);
            commanderService.SaveChanges();
            return Results.NoContent();
        });
        return group;
    }

}
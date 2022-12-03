using TerminalCommandsApi.Domain.Dto.Response;
using TerminalCommandsApi.Domain.Models;

namespace TerminalCommandsApi.Extensions;

public static class CommandExtensions
{


    public static CommandReadDto ToReadDto(this Command command) => new CommandReadDto
    {
        Name = command.Name,
        Description = command.Description,
        Platform = command.Platform,
    };
}
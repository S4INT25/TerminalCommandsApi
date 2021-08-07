using System.ComponentModel.DataAnnotations;
using TerminalCommands.Domain.Enums;

namespace TerminalCommandsApi.Dto.Request
{
    public class CommandCreateDto
    {
        [Required] public string Name { get; set; }

        [Required] public string Description { get; set; }

        [Required] public Platform Platform { get; set; }
    }
}
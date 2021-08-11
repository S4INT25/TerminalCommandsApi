using System.ComponentModel.DataAnnotations;
using TerminalCommandsApi.Domain.Enums;

namespace TerminalCommandsApi.Domain.Dto.Request
{
    public class CommandCreateDto
    {
        [Required] public string Name { get; set; }

        [Required] public string Description { get; set; }

        [Required] public Platform Platform { get; set; }
    }
}
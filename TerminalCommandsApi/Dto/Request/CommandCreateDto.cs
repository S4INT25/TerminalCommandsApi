using TerminalCommands.Domain.Enums;
using TerminalCommands.Domain.Models;

namespace TerminalCommands.Dto
{
    public class CommandCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Platform Platform { get; set; }
    }
}
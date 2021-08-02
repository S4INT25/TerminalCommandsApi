using TerminalCommands.Domain.Enums;

namespace TerminalCommands.Dto
{
    public class CommandReadDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Platform Platform { get; set; }
    }
}
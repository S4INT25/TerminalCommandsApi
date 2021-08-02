using TerminalCommands.Domain.Enums;

namespace TerminalCommands.Dto
{
    public class CommandUpdateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Platform Platform { get; set; }
    }
}
using TerminalCommandsApi.Domain.Enums;

namespace TerminalCommandsApi.Domain.Dto.Response
{
    public class CommandReadDto
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required Platform Platform { get; set; }
    }
}
using TerminalCommandsApi.Domain.Enums;

namespace TerminalCommandsApi.Domain.Dto.Response
{
    public class CommandReadDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Platform Platform { get; set; }
    }
}
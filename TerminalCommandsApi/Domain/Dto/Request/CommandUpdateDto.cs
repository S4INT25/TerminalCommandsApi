using TerminalCommandsApi.Domain.Enums;

namespace TerminalCommandsApi.Domain.Dto.Request
{
    public class CommandUpdateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Platform Platform { get; set; }
    }
}
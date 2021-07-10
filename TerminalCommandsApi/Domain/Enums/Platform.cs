using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TerminalCommands.Domain.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Platform
    {
        Linux,
        Windows,
        Mac
    }
}
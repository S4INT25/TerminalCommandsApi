using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TerminalCommandsApi.Domain.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Platform
    {
        Linux,
        Windows,
        Mac
    }
}
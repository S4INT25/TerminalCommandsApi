using System.Text.Json.Serialization;


namespace TerminalCommandsApi.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Platform
{
    Linux,
    Windows,
    Mac
}
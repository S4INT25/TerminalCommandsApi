using Newtonsoft.Json;

namespace TerminalCommands.Domain.Models
{
    public class CommanderException
    {
        public int StatusCode { get; init; }
        public string Message { get; init; }

      


        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
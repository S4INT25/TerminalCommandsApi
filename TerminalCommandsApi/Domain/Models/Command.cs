using System.ComponentModel.DataAnnotations;
using TerminalCommandsApi.Domain.Enums;

namespace TerminalCommandsApi.Domain.Models
{


    public class Command
    {
        public int Id { get; set; }

        [Required] public string Name { get; set; }
        [Required] public string Description { get; set; }
        [Required] public Platform Platform { get; set; }

    }
}
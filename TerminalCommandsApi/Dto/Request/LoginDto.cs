using System.ComponentModel.DataAnnotations;

namespace TerminalCommandsApi.Dto.Request
{
    public class LoginDto
    {

        [Required] [EmailAddress] public string Email { get; set; }
        [Required] public string Password { get; set; }
    }
}
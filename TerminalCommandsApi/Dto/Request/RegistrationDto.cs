using System.ComponentModel.DataAnnotations;

namespace TerminalCommandsApi.Dto.Request
{
    public class RegistrationDto
    {
        [Required] [EmailAddress] public string Email { get; set; }
        [Required] public string UserName { get; set; }
        [Required] public string Password { get; set; }

    }
}
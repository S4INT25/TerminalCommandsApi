using System.ComponentModel.DataAnnotations;

namespace TerminalCommandsApi.Domain.Dto.Request
{
    public class RefreshTokenRequest
    {
        [Required] public string Token { get; set; }

        [Required] public string RefreshToken { get; set; }
    }
}
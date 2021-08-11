using System.Threading.Tasks;
using TerminalCommandsApi.Domain.Dto.Request;
using TerminalCommandsApi.Dto.Response;

namespace TerminalCommandsApi.Domain.Interfaces
{
    public interface IAuthService
    {


        public Task<AuthResult> RegisterAsync(RegistrationDto registrationDto);
        public Task<AuthResult> LoginAsync(LoginRequest loginRequest);

        public Task<AuthResult> RefreshTokenAsync(RefreshTokenRequest tokenRequest);


    }


}
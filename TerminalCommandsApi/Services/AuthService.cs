using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TerminalCommandsApi.Configurations;
using TerminalCommandsApi.Data.DbContext;
using TerminalCommandsApi.Domain.Dto.Request;
using TerminalCommandsApi.Domain.Interfaces;
using TerminalCommandsApi.Domain.Models;
using TerminalCommandsApi.Dto.Response;

namespace TerminalCommandsApi.Services
{
    public class AuthService : IAuthService
    {

        private readonly CommanderContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtConfig _jwtConfig;
        private readonly TokenValidationParameters _tokenValidationParams;
        private readonly JwtSecurityTokenHandler _jwtTokenHandler;

        public AuthService(CommanderContext context, UserManager<IdentityUser> userManager,
            IOptionsMonitor<JwtConfig> optionsMonitor,
            TokenValidationParameters tokenValidationParams)
        {
            _jwtConfig = optionsMonitor.CurrentValue;
            _tokenValidationParams = tokenValidationParams;
            _userManager = userManager;
            _dbContext = context;
            _jwtTokenHandler = new JwtSecurityTokenHandler();
        }


        public async Task<AuthResult> RegisterAsync(RegistrationDto registrationDto)
        {
            var existingUser = await _userManager.FindByEmailAsync(registrationDto.Email);

            if (existingUser != null)
            {
                throw new Exception("Email already in use");
            }

            var newUser = new IdentityUser { Email = registrationDto.Email, UserName = registrationDto.UserName };

            var isCreated = await _userManager.CreateAsync(newUser, registrationDto.Password);

            if (!isCreated.Succeeded)
                throw new Exception("Failed to create user");
            var securityToken = GenerateJwtToken(newUser, _jwtConfig.Secret);
            var jwtToken = _jwtTokenHandler.WriteToken(securityToken);
            var refreshToken = new RefreshToken()
            {
                JwtId = securityToken.Id,
                IsUsed = false,
                IsRevoked = false,
                UserId = newUser.Id,
                AddedDate = DateTimeOffset.Now,
                ExpiryDate = DateTimeOffset.Now.AddMonths(6),
                Token = GenerateRandomString()
            };

            await _dbContext.RefreshTokens.AddAsync(refreshToken);
            await _dbContext.SaveChangesAsync();

            return new AuthResult
            {
                Token = jwtToken,
                Success = true,
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<AuthResult> LoginAsync(LoginRequest loginRequest)
        {
            var existingUser = await _userManager.FindByEmailAsync(loginRequest.Email);

            if (existingUser == null)
            {
                throw new Exception("Invalid login request user doesn't exist");
            }

            var isCorrect = await _userManager.CheckPasswordAsync(existingUser, loginRequest.Password);

            if (!isCorrect)
            {
                throw new Exception("Invalid user password");
            }

            var securityToken = GenerateJwtToken(existingUser, _jwtConfig.Secret);

            var refreshToken = new RefreshToken()
            {
                JwtId = securityToken.Id,
                IsUsed = false,
                IsRevoked = false,
                UserId = existingUser.Id,
                AddedDate = DateTimeOffset.Now,
                ExpiryDate = DateTimeOffset.Now.AddMonths(6),
                Token = GenerateRandomString()
            };
            var jwtToken = _jwtTokenHandler.WriteToken(securityToken);
            await _dbContext.RefreshTokens.AddAsync(refreshToken);
            await _dbContext.SaveChangesAsync();
            return new AuthResult()
            {
                Token = jwtToken,
                Success = true,
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<AuthResult> RefreshTokenAsync(RefreshTokenRequest tokenRequest)
        {
            return await VerifyAndRefreshToken(tokenRequest);
        }

        private static SecurityToken GenerateJwtToken(IdentityUser user, string secret)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.Now.AddYears(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };


            return jwtTokenHandler.CreateToken(tokenDescriptor);
        }

        private async Task<AuthResult> VerifyAndRefreshToken(RefreshTokenRequest tokenRequest)
        {
            // _tokenValidationParams.ValidateLifetime = false;
            try
            {
                // Validation 1 - Validation JWT token format
                var tokenInVerification = _jwtTokenHandler.ValidateToken(
                    tokenRequest.Token,
                    _tokenValidationParams,
                    out var validatedToken);

                // Validation 2 - Validate encryption alg
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(
                        SecurityAlgorithms.HmacSha256,
                        StringComparison.InvariantCultureIgnoreCase);

                    if (result == false)
                    {
                        return null;
                    }
                }

                // Validation 3 - validate expiry date
                var utcExpiryDate = tokenInVerification?.Claims
                                        .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp)?.Value ??
                                    throw new NullReferenceException();

                var expiryDate = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(utcExpiryDate));

                if (expiryDate > DateTimeOffset.Now)
                {
                    return new AuthResult()
                    {
                        Token = tokenRequest.Token,
                        RefreshToken = tokenRequest.RefreshToken,
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Token has not yet expired"
                        }
                    };
                }

                // validation 4 - validate existence of the token
                var storedToken =
                    await _dbContext.RefreshTokens.SingleOrDefaultAsync(token => token.JwtId == validatedToken.Id);


                if (storedToken == null)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Token does not exist"
                        }
                    };
                }

                // Validation 5 - validate if used
                if (storedToken.IsUsed)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Token has been used"
                        }
                    };
                }

                // Validation 6 - validate if revoked
                if (storedToken.IsRevoked)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Token has been revoked"
                        }
                    };
                }

                // Validation 7 - validate the id
                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)!
                    .Value;

                if (storedToken.JwtId != jti)
                {
                    return new AuthResult()
                    {
                        Token = tokenRequest.Token,
                        RefreshToken = tokenRequest.RefreshToken,
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Token doesn't match"
                        }
                    };
                }

                // update current token 
                storedToken.IsUsed = true;

                _dbContext.RefreshTokens.Update(storedToken);
                await _dbContext.SaveChangesAsync();

                // Generate a new token
                var dbUser = await _userManager.FindByIdAsync(storedToken.UserId);
                var token = _jwtTokenHandler.WriteToken(GenerateJwtToken(dbUser, _jwtConfig.Secret));
                return new AuthResult
                {
                    Token = token, Success = true, RefreshToken = storedToken.Token
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                if (ex.Message.Contains("Lifetime validation failed. The token is expired."))
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Token has expired please re-login"
                        }
                    };
                }

                return new AuthResult()
                {
                    Success = false,
                    Errors = new List<string>
                    {
                        "Something went wrong."
                    }
                };
            }
        }


        private string GenerateRandomString(int length = 32)
        {
            var random = new Random();
            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(characters, length)
                .Select(x => x[random.Next(x.Length)])
                .ToArray()) + Guid.NewGuid().ToString()
                .Replace("-", "");
        }
    }
}
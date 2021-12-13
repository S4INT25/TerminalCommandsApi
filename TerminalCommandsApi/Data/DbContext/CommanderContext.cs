using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using TerminalCommandsApi.Domain.Models;

namespace TerminalCommandsApi.Data.DbContext
{
    public class CommanderContext : IdentityDbContext
    {
        public CommanderContext(DbContextOptions<CommanderContext> options) : base(options)
        {
        }

        public DbSet<Command> Commands { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }


    }
}
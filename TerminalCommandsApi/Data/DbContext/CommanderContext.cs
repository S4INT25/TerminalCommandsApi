using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TerminalCommandsApi.Domain.Models;

namespace TerminalCommandsApi.Data.DbContext;

public sealed class CommanderContext : IdentityDbContext
{
    public CommanderContext(DbContextOptions<CommanderContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Command> Commands => Set<Command>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
}
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TerminalCommands.Domain.Models;

namespace TerminalCommandsApi.Data
{
    public class CommanderContext : IdentityDbContext
    {
        public CommanderContext(DbContextOptions<CommanderContext> options) : base(options)
        {
        }

        public DbSet<Command> Commands { get; set; }


    }
}
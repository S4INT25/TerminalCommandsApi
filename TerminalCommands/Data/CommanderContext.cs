using Microsoft.EntityFrameworkCore;
using TerminalCommands.Models;

namespace TerminalCommands.Data
{
    public class CommanderContext : DbContext
    {
        public CommanderContext(DbContextOptions<CommanderContext> options) : base(options)
        {
        }

        public DbSet<Command> Commands { get; set; }


    }
}
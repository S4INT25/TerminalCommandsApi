using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TerminalCommandsApi.Data.DbContext;
using TerminalCommandsApi.Domain.Interfaces;
using TerminalCommandsApi.Domain.Models;


namespace TerminalCommandsApi.Services
{
    public class CommanderService : ICommanderService
    {
        private readonly CommanderContext _context;


        public CommanderService(CommanderContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Command>> GetCommands()
        {
            return await _context.Commands.ToListAsync();
        }

        public async Task<Command?> GetCommandById(int id)
        {
            return await _context.Commands.FirstOrDefaultAsync(command => command.Id.Equals(id));
        }

        public void CreateCommand(Command command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            _context.Add(command);
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }

        public void DeleteCommand(Command command)
        {
            _context.Remove(command);
        }

        public void DeleteAllCommands()
        {
            _context.RemoveRange(_context.Commands);
        }
    }
}
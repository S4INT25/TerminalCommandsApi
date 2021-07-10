using System;
using System.Collections.Generic;
using System.Linq;
using TerminalCommands.Domain.Models;

namespace TerminalCommands.Data
{
    public class SqlCommanderRepo : ICommanderRepo
    {
        private readonly CommanderContext _context;


        public SqlCommanderRepo(CommanderContext context)
        {
            _context = context;
        }

        public ICollection<Command> GetCommands()
        {
            return _context.Commands.ToList();
        }

        public Command GetCommandById(int id)
        {
            return _context.Commands.FirstOrDefault(command => command.Id.Equals(id));
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
            return (_context.SaveChanges() >= 0);
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
using System.Collections.Generic;
using TerminalCommandsApi.Domain.Enums;
using TerminalCommandsApi.Domain.Interfaces;
using TerminalCommandsApi.Domain.Models;

namespace TerminalCommandsApi.Services
{
    public class MockCommanderRepo : ICommanderRepo
    {

        private readonly List<Command> _commands = new()
        {
            new Command { Id = 1, Name = "dir", Description = "Directory listing", Platform = Platform.Windows },
            new Command { Id = 2, Name = "ls-ls", Description = "Directory listing", Platform = Platform.Linux },
            new Command { Id = 3, Name = "ls", Description = "Directory listing", Platform = Platform.Mac }
        };


        public ICollection<Command> GetCommands()
        {
            return _commands;
        }

        public Command GetCommandById(int id)
        {
            return _commands.Find(command => command.Id.Equals(id));
        }

        public void CreateCommand(Command command)
        {
            throw new System.NotImplementedException();
        }

        public bool SaveChanges()
        {
            throw new System.NotImplementedException();
        }

        public void DeleteCommand(Command command)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAllCommands()
        {
            throw new System.NotImplementedException();
        }

    }
}
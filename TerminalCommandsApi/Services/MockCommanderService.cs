using System.Collections.Generic;
using System.Threading.Tasks;
using TerminalCommandsApi.Domain.Enums;
using TerminalCommandsApi.Domain.Interfaces;
using TerminalCommandsApi.Domain.Models;

namespace TerminalCommandsApi.Services
{
    public class MockCommanderService : ICommanderService
    {

        private readonly List<Command> _commands = new()
        {
            new Command { Id = 1, Name = "dir", Description = "Directory listing", Platform = Platform.Windows },
            new Command { Id = 2, Name = "ls-ls", Description = "Directory listing", Platform = Platform.Linux },
            new Command { Id = 3, Name = "ls", Description = "Directory listing", Platform = Platform.Mac }
        };


        public Task<ICollection<Command>> GetCommands()
        {
            throw new System.NotImplementedException();
        }

        public Task<Command> GetCommandById(int id)
        {
           
            throw new System.NotImplementedException();
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
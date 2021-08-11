using System.Collections.Generic;
using TerminalCommandsApi.Domain.Models;

namespace TerminalCommandsApi.Domain.Interfaces
{
    public interface ICommanderRepo
    {
        ICollection<Command> GetCommands();
        Command GetCommandById(int id);

        void CreateCommand(Command command);

        bool SaveChanges();

        void DeleteCommand(Command command);
        void DeleteAllCommands();
    }
}
using System.Collections;
using System.Collections.Generic;
using TerminalCommands.Models;

namespace TerminalCommands.Data
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
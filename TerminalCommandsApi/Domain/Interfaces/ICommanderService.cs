using System.Collections.Generic;
using System.Threading.Tasks;
using TerminalCommandsApi.Domain.Models;

namespace TerminalCommandsApi.Domain.Interfaces
{
    public interface ICommanderService
    {
        Task<ICollection<Command>> GetCommands();
        Task<Command> GetCommandById(int id);

        void CreateCommand(Command command);

        bool SaveChanges();

        void DeleteCommand(Command command);
        void DeleteAllCommands();
    }
}
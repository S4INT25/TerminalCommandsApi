using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using TerminalCommandsApi.Domain.Dto.Response;
using TerminalCommandsApi.Domain.Models;

namespace TerminalCommandsApi.Hubs
{


    public interface IDataBaseHub
    {
        [HubMethodName("dataAdded")]
        public Task DataAdded(Command message);

        [HubMethodName("getAllCommands")]
        public Task GetAllCommands(List<CommandReadDto> commands);

    }
}
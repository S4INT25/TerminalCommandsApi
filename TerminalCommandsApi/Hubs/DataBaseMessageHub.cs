using System;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TerminalCommandsApi.Data.DbContext;
using TerminalCommandsApi.Domain.Dto.Response;


namespace TerminalCommandsApi.Hubs
{

    public class DataBaseMessageHub : Hub<IDataBaseHub>
    {

        private readonly CommanderContext _dbContext;

        public DataBaseMessageHub(CommanderContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task SendAllCommands()
        {
            var commands = await _dbContext.Commands
                .ProjectToType<CommandReadDto>()
                .ToListAsync();

            await Clients.All.GetAllCommands(commands);
        }

        public override Task OnConnectedAsync()
        {
            Console.WriteLine("client connected");
            return Task.CompletedTask;
        }
    }
}
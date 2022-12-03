using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TerminalCommandsApi.Data.DbContext;
using TerminalCommandsApi.Extensions;


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
                .Select(x => x.ToReadDto())
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
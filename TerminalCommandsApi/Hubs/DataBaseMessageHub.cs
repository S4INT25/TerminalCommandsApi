using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using TerminalCommandsApi.Data.DbContext;
using TerminalCommandsApi.Domain.Dto.Response;


namespace TerminalCommandsApi.Hubs
{

    public class DataBaseMessageHub : Hub<IDataBaseHub>
    {

        private readonly CommanderContext _dbContext;
        private readonly IMapper _mapper;

        public DataBaseMessageHub(CommanderContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        public void SendAllCommands()
        {
            var commands = _dbContext.Commands.ToList();

            Clients.All.GetAllCommands(_mapper.Map<List<CommandReadDto>>(commands));
        }

        public override Task OnConnectedAsync()
        {
            Console.WriteLine("client connected");
            return Task.CompletedTask;
        }
    }
}
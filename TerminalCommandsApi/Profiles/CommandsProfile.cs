using AutoMapper;
using TerminalCommandsApi.Domain.Dto.Request;
using TerminalCommandsApi.Domain.Dto.Response;
using TerminalCommandsApi.Domain.Models;

namespace TerminalCommandsApi.Profiles
{
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            CreateMap<Command, CommandReadDto>();
            CreateMap<CommandCreateDto, Command>().ReverseMap();
            CreateMap<CommandUpdateDto, Command>().ReverseMap();
        }

    }
}
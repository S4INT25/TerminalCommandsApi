using AutoMapper;
using TerminalCommands.Dto;
using TerminalCommands.Models;

namespace TerminalCommands.Profiles
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
using AutoMapper;
using TerminalCommands.Domain.Models;
using TerminalCommands.Dto;

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
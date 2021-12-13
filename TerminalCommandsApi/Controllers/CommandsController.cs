using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using TerminalCommandsApi.Domain.Dto.Request;
using TerminalCommandsApi.Domain.Dto.Response;
using TerminalCommandsApi.Domain.Interfaces;
using TerminalCommandsApi.Domain.Models;
using TerminalCommandsApi.Extensions;
using TerminalCommandsApi.Hubs;

namespace TerminalCommandsApi.Controllers

{


    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CommandsController : ControllerBase
    {
        private readonly ICommanderRepo _commanderRepo;
        private readonly IMapper _mapper;
        private readonly IHubContext<DataBaseMessageHub, IDataBaseHub> _hubContext;


        public CommandsController(ICommanderRepo commanderRepo, IMapper mapper,
            IHubContext<DataBaseMessageHub, IDataBaseHub> hubContext)
        {
            _commanderRepo = commanderRepo;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        //Get api/commands
        [HttpGet]
        public ActionResult<ICollection<CommandReadDto>> GetAllCommands()
        {
            Console.WriteLine($"The user id is {HttpContext.GetUserId()}");
            var commands = _commanderRepo.GetCommands();
            if (commands.Count.Equals(0))
            {
                return NoContent();
            }


            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
        }

        //Get api/commands/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CommandReadDto>> GetCommand(int id)
        {
            var command = _commanderRepo.GetCommandById(id);
            if (command == null)
            {
                return NotFound();
            }

            await _hubContext.Clients.All.DataAdded(command);

            return Ok(_mapper.Map<CommandReadDto>(command));
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand(CommandCreateDto commandCreateDto)
        {
            var commandModel = _mapper.Map<Command>(commandCreateDto);
            _commanderRepo.CreateCommand(commandModel);

            if (!_commanderRepo.SaveChanges()) return BadRequest();

            return Created("test", _mapper.Map<CommandReadDto>(commandModel));
        }

        [HttpPut("{id:int}")]
        public ActionResult UpdateCommand(int id, CommandUpdateDto commandUpdateDto)
        {
            var command = _commanderRepo.GetCommandById(id);
            if (command == null)
            {
                return NotFound();
            }

            _mapper.Map(commandUpdateDto, command);
            _commanderRepo.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public ActionResult PartialUpdateCommand(int id, JsonPatchDocument<CommandUpdateDto> patchDocument)
        {
            var command = _commanderRepo.GetCommandById(id);
            if (command == null)
            {
                return NotFound();
            }

            var commandToPatch = _mapper.Map<CommandUpdateDto>(command);
            patchDocument.ApplyTo(commandToPatch, ModelState);
            if (!TryValidateModel(ModelState))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(commandToPatch, command);
            _commanderRepo.SaveChanges();
            return NoContent();
        }


        [HttpDelete("{id:int}")]
        public ActionResult DeleteCommand(int id)
        {
            var command = _commanderRepo.GetCommandById(id);
            if (command == null)
            {
                return NotFound();
            }

            _commanderRepo.DeleteCommand(command);
            _commanderRepo.SaveChanges();
            return NoContent();
        }


        [HttpDelete]
        public ActionResult DeleteAllCommands()
        {
            _commanderRepo.DeleteAllCommands();
            _commanderRepo.SaveChanges();
            return NoContent();
        }

    }
}
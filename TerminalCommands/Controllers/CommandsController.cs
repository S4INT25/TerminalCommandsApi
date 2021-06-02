using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TerminalCommands.Data;
using TerminalCommands.Dto;
using TerminalCommands.Models;

namespace TerminalCommands.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandController : ControllerBase
    {
        private readonly ICommanderRepo _commanderRepo;
        private readonly IMapper _mapper;

        public CommandController(ICommanderRepo commanderRepo, IMapper mapper)
        {
            _commanderRepo = commanderRepo;
            _mapper = mapper;
        }

        //Get api/commands
        [HttpGet]
        public ActionResult<ICollection<CommandReadDto>> GetAllCommands()
        {
            var commands = _commanderRepo.GetCommands();
            if (commands.Count.Equals(0))
            {
                return NoContent();
            }

            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
        }

        //Get api/commands/{id}
        [HttpGet("{id:int}")]
        public ActionResult<CommandReadDto> GetCommand(int id)
        {
            var command = _commanderRepo.GetCommandById(id);
            if (command == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CommandReadDto>(command));
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand(CommandCreateDto commandCreateDto)
        {
            var commandModel = _mapper.Map<Command>(commandCreateDto);
            _commanderRepo.CreateCommand(commandModel);

            if (_commanderRepo.SaveChanges())
            {
                return Created("test", _mapper.Map<CommandReadDto>(commandModel));
            }

            return BadRequest();
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
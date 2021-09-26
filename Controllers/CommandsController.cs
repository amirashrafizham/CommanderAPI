using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Commander.Data;
using Commander.Models;
using Commander.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;

namespace Commander.Controllers
{
    [ApiController]
    [Route("api/commands")]
    public class CommandsController : ControllerBase
    {
        private readonly ICommanderRepo _repository;
        private readonly IMapper _mapper;

        public CommandsController(ICommanderRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        //GET api/commands
        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands()
        {
            var commandItems = _repository.GetAllCommands();
            if (commandItems != null)
            {
                return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
            }
            return NotFound();
        }

        //GET api/commands/{id}
        [HttpGet("{id}", Name = "GetCommandById")] //Name is used for the POST Request
        public ActionResult<CommandReadDto> GetCommandById(int id)
        {
            var commandItems = _repository.GetCommandById(id);
            if (commandItems != null)
            {
                return Ok(_mapper.Map<CommandReadDto>(commandItems));
            }
            return NotFound();
        }

        [HttpPost]
        public ActionResult<CommandReadDto> AddCommand(PostCommandDto postCmd)
        {
            var cmd = _mapper.Map<Command>(postCmd);
            _repository.CreateCommand(cmd);
            _repository.SaveChanges();
            var commandReadDto = _mapper.Map<CommandReadDto>(cmd);

            //Part of REST's specification, after you have created a resource you need to return back the URI to the client. Check your POSTMMAN under Headers
            return CreatedAtRoute(nameof(GetCommandById), new { Id = commandReadDto.Id }, commandReadDto);
            //Checkout CreatedAtAction as well
        }

        [HttpDelete("{id}")]
        public ActionResult<bool> RemoveCommand(int id)
        {
            var getId = _repository.GetCommandById(id);
            if (getId == null)
            {
                return NotFound();
            }
            _repository.DeleteCommand(getId);
            return Ok(_repository.SaveChanges());
        }

        [HttpPut("{id}")]
        public ActionResult UpdateCommand(int id, UpdateCommandDto updateCmd)
        {
            var getId = _repository.GetCommandById(id);
            if (getId == null)
            {
                return NotFound();
            }
            _mapper.Map(updateCmd, getId); //this is automatically doing the mapping to the databse
            _repository.UpdateCommand(getId);//although it does nothing, it's good practice to put it in here
            _repository.SaveChanges();
            return NoContent(); //HTTP 204 return
        }

        //Compared to Put, Patch allows partial update. You don't need to provide the whole object itself. The client will need to send an array '[{}]'  

        [HttpPatch("{id}")]
        public ActionResult PatchCommand(int id, JsonPatchDocument<UpdateCommandDto> patchCmd)
        {

            //Command Type
            var getId = _repository.GetCommandById(id);
            if (getId == null)
            {
                return NotFound();
            }

            //UpdateCommandDto Type
            var commandPatch = _mapper.Map<UpdateCommandDto>(getId);

            //To apply JSON PATCH operations
            patchCmd.ApplyTo(commandPatch, ModelState);
            if (!TryValidateModel(commandPatch))
            {
                return ValidationProblem(ModelState);
            }

            //Command Type
            _mapper.Map(commandPatch, getId);
            _repository.SaveChanges();
            return NoContent();

        }
    }
}
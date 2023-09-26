using Dbragas.Interfaces;
using DBragas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;

namespace Dbragas.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class TypeClientController : ControllerBase
    {

        public readonly ITypeClientRepository _typeRepository;
        public TypeClientController(ITypeClientRepository typeRepository)
        {
            _typeRepository = typeRepository;

        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddType([FromBody] TypeClientCreateDto type)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var existingType = await _typeRepository.GetByName(type.Name);
                if (existingType != null)
                {
                    return BadRequest($"O tipo {existingType} com o id {existingType.Id} já existe ");
                }
                else if (existingType == null)
                {
                    var typeClient = new TypeClients
                    {
                        Id = Guid.NewGuid(),
                        Name = type.Name.ToUpper(),
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsActive = true
                    };
                    _typeRepository.Add(typeClient);
                    if (await _typeRepository.SaveAllAsync())
                    {
                        return Ok("Tipo salvo com sucesso");
                    }
                }
            }
            catch (DbException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error: An error occurred while processing the request.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error: An error occurred while processing the request.");
            }

            return StatusCode(StatusCodes.Status500InternalServerError, "Error: An error occurred while processing the request.");

        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult> GetById(Guid id)
        {
            try
            {
                var type = await _typeRepository.GetById(id);
                if (type != null)
                { 
                    return Ok(type);
                }
                return NotFound("Tipo não encontrado");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error: An error occurred while processing the request.");

            }
        }
        [HttpGet("findAll")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Type>>> GetAllTypes()
        {
            var types = await _typeRepository.GetAllAsync();
            if (types != null)
            {
                return NotFound("Tipos não encontrados");
            }
            return Ok(types);
        }
        [HttpPatch("{id}")]
        [Authorize]
        public async Task<ActionResult> PatchType(Guid id, TypeClientCreateDto type)
        {
            if (type == null || !type.GetType().GetProperties().Any(prop => prop.GetValue(type) != null))
            {
                return BadRequest("Request vazio");
            }
            var existingType = await _typeRepository.GetById(id);
            if(type == null)
            {
                return NotFound("Tipo não encontrado");
            }
            existingType.UpdatedAt = DateTime.UtcNow;
            if (await _typeRepository.SaveAllAsync())
            {
                return Ok("Tipo Modificado com sucesso");
            } ;
            return StatusCode(StatusCodes.Status500InternalServerError, "Error: An error occurred while processing the request.");
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteType(Guid id)
        {
            var type = await _typeRepository.GetById(id); 
            if (type == null) 
            {
                return NotFound("Tipo não encontrado"); 
            }
            _typeRepository.Delete(type);
            if(await _typeRepository.SaveAllAsync())
            {
                return Ok("Tipo deletado com sucesso");
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "Error: An error occurred while processing the request.");

        }
    }
}
        






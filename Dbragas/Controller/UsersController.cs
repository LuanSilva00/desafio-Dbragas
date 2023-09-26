using Dbragas.Interfaces;
using Dbragas.Repositories;
using DBragas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dbragas.Controller
{
    [ApiController]
    [Route("api/[controller]/")]
    public class UsersController : ControllerBase
    {
        public readonly IUserRepository _usersRepository;
        public UsersController(IUserRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        [HttpGet("findAll")]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        {
            var users = await _usersRepository.GetAllAsync();
            if (users == null || !users.Any())
            {
                return NotFound("Usuários não encontrados");
            }

            return Ok(users);
        }
        [HttpPost]
        public async Task<ActionResult> AddUser(Users users)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                _usersRepository.Add(users);
                if (await _usersRepository.SaveAllAsync())
                {
                    return Ok("User Saved Successfully");
                }
            }
            catch (DbUpdateException)
            {
                return BadRequest("Error: Duplicate primary key. User not saved.");
            }
            catch (Exception)
            {
                return BadRequest("Error: An error occurred while saving the user.");
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "Error: An error occurred while processing the request.");
        }
        [HttpPatch]
        public async Task<ActionResult> PatchUser(Users users)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                _usersRepository.Patch(users);
                if (await _usersRepository.SaveAllAsync())
                {
                    return Ok("User updated sucessfuly");
                }
            }
            catch (Exception)
            {
                return BadRequest("Error: An error occurred while saving the user.");
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "Error: An error occurred while processing the request.");
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            var user = await _usersRepository.GetById(id);
            if(user == null)
            {
                return NotFound("Usuário nao encontrado");
            }
            _usersRepository.Delete(user);

            if(await _usersRepository.SaveAllAsync()) {
                return Ok("Usuário deletado com sucesso!");
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "Error: An error occurred while processing the request.");
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            try
            {
                await _usersRepository.GetById(id);
            }
            catch (Exception)
            {
                return NotFound("Usuário não encontrado");
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "Error: An error occurred while processing the request.");
        }
    }
}

using Dbragas.Interfaces;
using Dbragas.Repositories;
using DBragas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;

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

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddUser(Users users)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(users.Password))
                {
                    throw new InvalidOperationException("O campo Senha não pode estar vazio");
                }
                var existingUserByEmail = await _usersRepository.GetByEmail(users.Email);
                var existingUserByUsername = await _usersRepository.GetByUsername(users.Username);

                if (existingUserByEmail != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error: An error occurred while processing the request.");
                }

                if (existingUserByUsername != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error: An error occurred while processing the request.");
                }

                users.Id = Guid.NewGuid();
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(users.Password);
                users.Password = hashedPassword;
                users.CreatedAt = DateTime.UtcNow;
                users.UpdatedAt = DateTime.UtcNow;
                users.IsActive = true;
                _usersRepository.Add(users);

                if (await _usersRepository.SaveAllAsync())
                {
                    return Ok("Usuário salvo com sucesso");
                }
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error: An error occurred while processing the request.");
            }
            catch (Exception)
            {
                return BadRequest("Error: An error occurred while saving the user.");
            }

            return StatusCode(StatusCodes.Status500InternalServerError, "Error: An error occurred while processing the request.");
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult> GetById(Guid id)
        {
            try
            {
                var user = await _usersRepository.GetById(id);
                if (user != null)
                {
                    user.Password = null;
                    return Ok(user);
                }
                else
                {
                    return NotFound("Usuário não encontrado");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error: An error occurred while processing the request.");
            }
        }
        [HttpGet("findAll")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        {
            var users = await _usersRepository.GetAllAsync();

            if (users == null || !users.Any())
            {
                return NotFound("Usuários não encontrados");
            }
            var newArray = users.Select(user =>
            {
                user.Password = null;
                return user;
            }).ToList();

            return Ok(newArray);
        }
        [HttpPatch("{id}")]
        [Authorize]
        public async Task<ActionResult> PatchUser(Guid id, Users updatedUser)
        {
            var existingUser = await _usersRepository.GetById(id);

            if (existingUser == null)
            {
                return NotFound("User not found");
            }

            existingUser.Name = updatedUser.Name;
            existingUser.Email = updatedUser.Email;
            existingUser.Username = updatedUser.Username;
            existingUser.UpdatedAt = DateTime.UtcNow;

            if (await _usersRepository.SaveAllAsync())
            {
                return Ok("User updated successfully");
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "Error: An error occurred while processing the request.");
        }
        [HttpDelete("{id}")]
        [Authorize]
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
    }
}

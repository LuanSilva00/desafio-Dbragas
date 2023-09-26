using Dbragas.Interfaces;
using Dbragas.Repositories;
using DBragas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using System.Data.Common;

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
        public async Task<ActionResult> AddUser([FromBody] CreateUserDto createUserDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingUserByEmail = await _usersRepository.GetByEmail(createUserDto.Email);
                var existingUserByUsername = await _usersRepository.GetByUsername(createUserDto.Username);

                if (existingUserByEmail != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error: An error occurred while processing the request.");
                }

                if (existingUserByUsername != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error: An error occurred while processing the request.");
                }
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);
                var newUser = new Users
                {
                    Name = createUserDto.Name,
                    Email = createUserDto.Email,
                    Password = hashedPassword,
                    Username = createUserDto.Username,
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsActive = true
                };


                _usersRepository.Add(newUser);

                if (await _usersRepository.SaveAllAsync())
                {
                    return Ok("Usuário salvo com sucesso");
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
        [Authorize]
        [HttpPatch("{id}")]
        public async Task<ActionResult> PatchUser(Guid id, [FromBody] PatchUserDTO userPatchDto)
        {
            if (userPatchDto == null || !userPatchDto.GetType().GetProperties().Any(prop => prop.GetValue(userPatchDto) != null))
            {
                return BadRequest("Request vazio");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingUser = await _usersRepository.GetById(id);

            if (existingUser == null)
            {
                return NotFound("User not found");
            }

            if (!string.IsNullOrEmpty(userPatchDto.Name))
            {
                existingUser.Name = userPatchDto.Name;
            }

            if (!string.IsNullOrEmpty(userPatchDto.Email))
            {
                existingUser.Email = userPatchDto.Email;
            }

            if (!string.IsNullOrEmpty(userPatchDto.Username))
            {
                existingUser.Username = userPatchDto.Username;
            }

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

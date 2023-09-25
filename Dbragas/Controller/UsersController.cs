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
            return Ok(await _usersRepository.GetAllAsync());
        }
        [HttpPost]
        public async Task<ActionResult> AddUser(Users users)
        {
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
        }
        [HttpPatch]
        public async Task<ActionResult> PatchUser(Users users)
        {
            try { _usersRepository.Patch(users);
            if (await _usersRepository.SaveAllAsync())
            {
                return Ok("User updated sucessfuly");
            }
        }
        }
    }
}

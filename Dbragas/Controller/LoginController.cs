using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Dbragas.Interfaces;
using Dbragas.Services;
using DBragas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Dbragas.Controller
{
    [ApiController]
    [Route("api/[controller]/")]
    public class LoginController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public LoginController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> AuthenticateAsync([FromBody] userLogin userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userRepository.GetByUsername(userModel.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(userModel.Password, user.Password))
            {
                return Unauthorized("Invalid username or password.");
            }

            var token = TokenService.GenerateToken(user);

            user.Password = "";

            return new
            {
                Token = token
            };
        }
    }
}

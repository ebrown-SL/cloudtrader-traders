using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CloudtraderTraders.Models;
using CloudtraderTraders.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CloudtraderTraders.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TraderController : ControllerBase
    {
        private readonly ITraderService _traderService;

        public TraderController(ITraderService traderService)
        {
            _traderService = traderService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticationModel model)
        {
            var user = await _traderService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest("Username or password is incorrect");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("SOMESECRETTHATSHOULDNOTBEHARDCODEDFORVERIFYINGAUTHENTICITYOFJWTTOKENS");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                Id = user.Id,
                Username = user.Username,
                Balance = user.Balance,
                Token = tokenString
            });
        }
        
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthenticationModel model)
        {
            var user = new Trader(model);

            await _traderService.Create(user, model.Password);
            return Ok();
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetOwnDetails()
        {
            var userId = User.Claims.Where(a => a.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var user = await _traderService.GetById(Convert.ToInt32(userId));

            return Ok(new 
            {
                Id = user.Id,
                Username = user.Username,
                Balance = user.Balance
            });
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _traderService.GetAll();
            return Ok(users);
        }

    }
}

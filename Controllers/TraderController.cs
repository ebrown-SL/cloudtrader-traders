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

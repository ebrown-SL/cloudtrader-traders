using System.Threading.Tasks;
using CloudTrader.Traders.Service.Models;
using CloudTrader.Traders.Services;
using Microsoft.AspNetCore.Mvc;

namespace CloudTrader.Traders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TraderController : ControllerBase
    {
        private readonly ITraderService _traderService;

        public TraderController(ITraderService traderService)
        {
            _traderService = traderService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrader(int id)
        {
            var trader = await _traderService.GetById(id);
            if (trader == null)
            {
                return NotFound();
            }

            return Ok(trader);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTrader(Trader Trader)
        {
            var existingTrader = await _traderService.GetById(Trader.Id);
            if (existingTrader != null)
            {
                return Conflict();
            }

            var trader = await _traderService.Create(Trader);

            return Ok(trader);
        }
    }
}

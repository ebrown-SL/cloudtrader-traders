using System.Threading.Tasks;
using CloudTrader.Traders.Service;
using Microsoft.AspNetCore.Mvc;
using CloudTrader.Traders.Models.Api;

namespace CloudTrader.Traders.Api.Controllers
{
    [Route("api/trader")]
    [ApiController]
    public class TraderController : ControllerBase
    {
        private readonly ITraderService _traderService;

        public TraderController(ITraderService traderService)
        {
            _traderService = traderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTraders()
        {
            var traders = await _traderService.GetTraders();

            return Ok(traders);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTrader()
        {
            var trader = await _traderService.CreateTrader();

            return Created($"/api/trader/{trader.Id}", trader);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrader(int id)
        {
            var trader = await _traderService.GetTrader(id);

            return Ok(trader);
        }

        [HttpPut("{id}/balance")]
        public async Task<IActionResult> SetBalance(int id, int balance)
        {
            var trader = await _traderService.SetBalance(id, balance);

            return Ok(trader);
        }

        [HttpGet("{id}/mines")]
        public async Task<IActionResult> GetTraderMines(int id)
        {
            var traderMines = await _traderService.GetTraderMines(id);

            return Ok(traderMines);
        }

        [HttpPost("{id}/mines/")]
        public async Task<IActionResult> AddTraderMine(int id, AddTraderMineModel mine)
        {
            var traderMines = await _traderService.AddTraderMine(id, mine);

            return Created("", traderMines);
        }

        [HttpGet("{id}/mines/{mineId}")]
        public async Task<IActionResult> AddTraderMine(int id, int mineId)
        {
            var traderMine = await _traderService.GetTraderMine(id, mineId);

            return Ok(traderMine);
        }

    }
}

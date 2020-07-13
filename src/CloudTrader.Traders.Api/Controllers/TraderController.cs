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
            var traders = new GetAllTradersResponseModel(await _traderService.GetTraders());

            return Ok(traders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrader(int id)
        {
            var trader = await _traderService.GetTrader(id);

            return Ok(trader);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTrader()
        {
            var trader = await _traderService.CreateTrader();

            return Created($"/api/trader/{trader.Id}",trader);
        }
    }
}

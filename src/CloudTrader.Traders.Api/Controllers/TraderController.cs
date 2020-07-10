using System.Threading.Tasks;
using CloudTrader.Traders.Models.Api;
using CloudTrader.Traders.Service;
using Microsoft.AspNetCore.Mvc;

namespace CloudTrader.Traders.Api.Controllers
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
            var trader = await _traderService.GetTrader(id);

            return Ok(trader);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTrader(TraderCreationModel creationModel)
        {
            var trader = await _traderService.CreateTrader(creationModel.Id);

            return Ok(trader);
        }
    }
}

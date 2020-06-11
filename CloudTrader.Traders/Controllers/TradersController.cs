using System.Collections.Generic;
using System.Threading.Tasks;
using CloudtraderTraders.Models;
using CloudtraderTraders.Services;
using Microsoft.AspNetCore.Mvc;

namespace CloudtraderTraders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TradersController : ControllerBase
    {
        private readonly ITraderService _traderService;

        public TradersController(ITraderService traderService)
        {
            _traderService = traderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TraderModel>>> GetAll()
        {
            var traders = await _traderService.GetAll();

            return Ok(traders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TraderModel>> GetTrader(int id)
        {
            var trader = await _traderService.GetById(id);

            if (trader == null)
            {
                return NotFound();
            }

            return Ok(trader);
        }

        [HttpPost]
        public async Task<ActionResult<TraderModel>> PostTrader(TraderModel traderModel)
        {
            var trader = await _traderService.Create(traderModel);

            return Ok(trader);
        }
    }
}

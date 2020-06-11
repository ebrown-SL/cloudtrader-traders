using System.Collections.Generic;
using System.Threading.Tasks;
using CloudtraderTraders.Models;
using CloudtraderTraders.Services;
using Microsoft.AspNetCore.Mvc;

namespace CloudtraderTraders.Controllers
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TraderModel>>> GetAll()
        {
            var traders = await _traderService.GetAll();

            return Ok(traders);
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
        public async Task<IActionResult> CreateTrader(TraderModel traderModel)
        {
            var existingTrader = await _traderService.GetById(traderModel.Id);
            if (existingTrader != null)
            {
                return Conflict();
            }

            var trader = await _traderService.Create(traderModel);

            return Ok(trader);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrader(int id, TraderModel traderModel)
        {
            if (id != traderModel.Id)
            {
                return BadRequest();
            }

            var trader = await _traderService.Update(traderModel);

            return Ok(trader);
        }
    }
}

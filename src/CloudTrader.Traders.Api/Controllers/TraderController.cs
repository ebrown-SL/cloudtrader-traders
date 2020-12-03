using CloudTrader.Traders.Models.Api.Request;
using CloudTrader.Traders.Models.Api.Response;
using CloudTrader.Traders.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;

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
        [SwaggerOperation(
            Summary = "Get all traders",
            Description = "Returns an object containing an array of all traders")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(GetAllTradersResponseModel))]
        public async Task<IActionResult> GetTraders()
        {
            var traders = await _traderService.GetTraders();

            return Ok(traders);
        }

        [HttpGet("mines/{mineId}")]
        [SwaggerOperation(
            Summary = "Get all traders by mine id",
            Description = "Returns all traders with stock in a given mine")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(GetTradersByMineIdResponseModel))]
        public async Task<IActionResult> GetTradersByMineId(Guid mineId)
        {
            var traders = await _traderService.GetTradersByMineId(mineId);

            return Ok(traders);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a new trader",
            Description = "Creates a new trader in the database and returns the new trader. Balance is optional.")]
        [SwaggerResponse(StatusCodes.Status201Created, "Trader created", typeof(TraderResponseModel))]
        public async Task<IActionResult> CreateTrader(CreateTraderRequestModel balance)
        {
            var trader = await _traderService.CreateTrader(balance);

            return Created($"/api/trader/{trader.Id}", trader);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Get a trader",
            Description = "Get a trader by ID from the database")]
        [SwaggerResponse(StatusCodes.Status200OK, "Trader found", typeof(TraderResponseModel))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Trader not found")]
        public async Task<IActionResult> GetTrader(Guid id)
        {
            var trader = await _traderService.GetTrader(id);

            return Ok(trader);
        }

        [HttpPut("{id}/balance")]
        [SwaggerOperation(
            Summary = "Set trader balance",
            Description = "Set the balance of a trader using their ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Balance updated", typeof(TraderResponseModel))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Trader not found")]
        public async Task<IActionResult> SetBalance(Guid id, SetTraderBalanceRequestModel balance)
        {
            var trader = await _traderService.SetBalance(id, balance);

            return Ok(trader);
        }

        [HttpPatch("{id}/balance")]
        [SwaggerOperation(
            Summary = "Incremente or decrement a trader's current balance",
            Description = "Returns the trader's updated balance - either incremented (provided amount is positive)" +
            " or decremented (provided amount is negative)"
            )]
        [SwaggerResponse(StatusCodes.Status200OK, "Balance updated", typeof(TraderResponseModel))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Trader not found")]
        public async Task<IActionResult> UpdateBalance(Guid id, UpdateTraderBalanceRequestModel amountToAdd)
        {
            var trader = await _traderService.UpdateBalance(id, amountToAdd);

            return Ok(trader);
        }

        [HttpGet("{id}/mines")]
        [SwaggerOperation(
            Summary = "Get all trader mine stocks",
            Description = "Returns an object containing an array of the mines and stock owned by a trader")]
        [SwaggerResponse(StatusCodes.Status200OK, "Mines found", typeof(GetTraderMinesResponseModel))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Trader not found")]
        public async Task<IActionResult> GetTraderMines(Guid id)
        {
            var traderMines = await _traderService.GetTraderMines(id);

            return Ok(traderMines);
        }

        [HttpPost("{id}/mines/")]
        [SwaggerOperation(
            Summary = "Set trader mine stock",
            Description = "Either creates or updates an existing mine stock for the trader")]
        [SwaggerResponse(StatusCodes.Status200OK, "Mine stock updated", typeof(GetTraderMinesResponseModel))]
        public async Task<IActionResult> SetTraderMines(Guid id, SetTraderMineRequestModel mine)
        {
            var traderMines = await _traderService.SetTraderMine(id, mine);

            return Ok(traderMines);
        }

        [HttpGet("{id}/mines/{mineId}")]
        [SwaggerOperation(
            Summary = "Get a trader mine stock",
            Description = "Returns information about a specific mine stock")]
        [SwaggerResponse(StatusCodes.Status200OK, "Mine stock found", typeof(CloudStockResponseModel))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Mine stock not found")]
        public async Task<IActionResult> GetTraderMine(Guid id, Guid mineId)
        {
            var traderMine = await _traderService.GetTraderMine(id, mineId);

            return Ok(traderMine);
        }

        [HttpDelete("{id}/mines/{mineId}")]
        [SwaggerOperation(
            Summary = "Remove a trader mine stock",
            Description = "If the mine stock exists, it is deleted. Returns the altered list of mine stocks")]
        [SwaggerResponse(StatusCodes.Status200OK, "Mine stock updated", typeof(GetTraderMinesResponseModel))]
        public async Task<IActionResult> DeleteTraderMine(Guid id, Guid mineId)
        {
            var traderMines = await _traderService.DeleteTraderMine(id, mineId);

            return Ok(traderMines);
        }
    }
}
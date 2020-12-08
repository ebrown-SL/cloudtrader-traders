using AutoMapper;
using CloudTrader.Traders.Api.Models.Request;
using CloudTrader.Traders.Api.Models.Response;
using CloudTrader.Traders.Domain.Models;
using CloudTrader.Traders.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudTrader.Traders.Api.Controllers
{
    [Route("api/trader")]
    [ApiController]
    public class TraderController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ITraderService traderService;

        public TraderController(IMapper mapper, ITraderService traderService)
        {
            this.mapper = mapper;
            this.traderService = traderService;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all traders",
            Description = "Returns an object containing an array of all traders")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(GetAllTradersResponseModel))]
        public async Task<IActionResult> GetTraders()
        {
            var traders = await traderService.GetTraders();
            var mappedTraders = mapper.Map<List<TraderResponseModel>>(traders);

            return Ok(new GetAllTradersResponseModel(mappedTraders));
        }

        [HttpGet("mines/{mineId}")]
        [SwaggerOperation(
            Summary = "Get all traders by mine id",
            Description = "Returns all traders with stock in a given mine")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(GetTradersByMineIdResponseModel))]
        public async Task<IActionResult> GetTradersByMineId(Guid mineId)
        {
            var traderStockInLocation = await traderService.GetTradersByMineId(mineId);
            var mappedTraderStocks = mapper.Map<List<TraderCloudStockResponseModel>>(traderStockInLocation);

            return Ok(new GetTradersByMineIdResponseModel(mappedTraderStocks));
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a new trader",
            Description = "Creates a new trader in the database and returns the new trader. Balance is optional.")]
        [SwaggerResponse(StatusCodes.Status201Created, "Trader created", typeof(TraderResponseModel))]
        public async Task<IActionResult> CreateTrader(CreateTraderRequestModel body)
        {
            var trader = await traderService.CreateTrader(body.Balance);

            return Created($"/api/trader/{trader.Id}", MapFromDbToTraderResponseModel(trader));
        }

        [HttpGet("{traderId}")]
        [SwaggerOperation(
            Summary = "Get a trader",
            Description = "Get a trader by ID from the database")]
        [SwaggerResponse(StatusCodes.Status200OK, "Trader found", typeof(TraderResponseModel))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Trader not found")]
        public async Task<IActionResult> GetTrader(Guid traderId)
        {
            var trader = await traderService.GetTrader(traderId);

            return Ok(MapFromDbToTraderResponseModel(trader));
        }

        [HttpPut("{traderId}/balance")]
        [SwaggerOperation(
            Summary = "Set trader balance",
            Description = "Set the balance of a trader using their ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Balance updated", typeof(TraderResponseModel))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Trader not found")]
        public async Task<IActionResult> SetBalance(Guid traderId, SetTraderBalanceRequestModel body)
        {
            var trader = await traderService.SetBalance(traderId, body.Balance);

            return Ok(MapFromDbToTraderResponseModel(trader));
        }

        [HttpPatch("{traderId}/balance")]
        [SwaggerOperation(
            Summary = "Incremente or decrement a trader's current balance",
            Description = "Returns the trader's updated balance - either incremented (provided amount is positive)" +
            " or decremented (provided amount is negative)"
            )]
        [SwaggerResponse(StatusCodes.Status200OK, "Balance updated", typeof(TraderResponseModel))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Trader not found")]
        public async Task<IActionResult> UpdateBalance(Guid id, UpdateTraderBalanceRequestModel body)
        {
            var trader = await traderService.UpdateBalance(id, body.AmountToAdd);

            return Ok(MapFromDbToTraderResponseModel(trader));
        }

        [HttpGet("{traderId}/mines")]
        [SwaggerOperation(
            Summary = "Get all trader mine stocks",
            Description = "Returns an object containing an array of the mines and stock owned by a trader")]
        [SwaggerResponse(StatusCodes.Status200OK, "Mines found", typeof(GetTraderMinesResponseModel))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Trader not found")]
        public async Task<IActionResult> GetTraderMines(Guid traderId)
        {
            var traderMines = await traderService.GetTraderMines(traderId);
            var mappedMines = mapper.Map<GetTraderMinesResponseModel>(traderMines);

            return Ok(mappedMines);
        }

        [HttpPost("{traderId}/mines/")]
        [SwaggerOperation(
            Summary = "Set trader mine stock",
            Description = "Either creates or updates an existing mine stock for the trader")]
        [SwaggerResponse(StatusCodes.Status200OK, "Mine stock updated", typeof(GetTraderMinesResponseModel))]
        public async Task<IActionResult> SetTraderMines(Guid traderId, SetTraderMineRequestModel body)
        {
            var traderMines = await traderService.SetTraderMine(traderId, body.MineId, body.Stock);

            return Ok(traderMines);
        }

        [HttpGet("{traderId}/mines/{mineId}")]
        [SwaggerOperation(
            Summary = "Get a trader mine stock",
            Description = "Returns information about a specific mine stock")]
        [SwaggerResponse(StatusCodes.Status200OK, "Mine stock found", typeof(CloudStockResponseModel))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Mine stock not found")]
        public async Task<IActionResult> GetTraderMine(Guid traderId, Guid mineId)
        {
            var traderMine = await traderService.GetTraderMine(traderId, mineId);
            var mappedMines = mapper.Map<GetTraderMinesResponseModel>(traderMine);

            return Ok(mappedMines);
        }

        [HttpDelete("{traderId}/mines/{mineId}")]
        [SwaggerOperation(
            Summary = "Remove a trader mine stock",
            Description = "If the mine stock exists, it is deleted. Returns the altered list of mine stocks")]
        [SwaggerResponse(StatusCodes.Status200OK, "Mine stock updated", typeof(GetTraderMinesResponseModel))]
        public async Task<IActionResult> DeleteTraderMine(Guid traderId, Guid mineId)
        {
            var trader = await traderService.DeleteTraderMine(traderId, mineId);
            var mappedResponse = mapper.Map<GetTraderMinesResponseModel>(trader);

            return Ok(mappedResponse);
        }

        public TraderResponseModel MapFromDbToTraderResponseModel(Trader dbModel)
        {
            return mapper.Map<TraderResponseModel>(dbModel);
        }
    }
}
using CloudTrader.Traders.Models.Api.Request;
using CloudTrader.Traders.Models.Api.Response;
using System;
using System.Threading.Tasks;

namespace CloudTrader.Traders.Service
{
    public interface ITraderService
    {
        Task<TraderResponseModel> CreateTrader(CreateTraderRequestModel balance);
        Task<TraderResponseModel> GetTrader(Guid id);
        Task<GetAllTradersResponseModel> GetTraders();
        Task<GetTradersByMineIdResponseModel> GetTradersByMineId(Guid mineId);
        Task<GetTraderMinesResponseModel> GetTraderMines(Guid id);
        Task<TraderResponseModel> SetBalance(Guid id, SetTraderBalanceRequestModel balance);
        Task<TraderResponseModel> UpdateBalance(Guid id, UpdateTraderBalanceRequestModel amount);
        Task<CloudStockResponseModel> GetTraderMine(Guid id, Guid mineId);
        Task<GetTraderMinesResponseModel> SetTraderMine(Guid id, SetTraderMineRequestModel mine);
        Task<GetTraderMinesResponseModel> DeleteTraderMine(Guid id, Guid mineId);
    }
}

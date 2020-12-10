using AutoMapper;
using CloudTrader.Traders.Api.Models.Response;
using CloudTrader.Traders.Domain.Models;

namespace CloudTrader.Traders.Api.Models
{
    public class TraderProfile : Profile
    {
        public TraderProfile()
        {
            CreateMap<TraderResponseModel, Trader>()
                .ReverseMap();

            CreateMap<Trader, GetTraderMinesResponseModel>()
                .ForMember(dest => dest.CloudStock, act => act.MapFrom(src => src.CloudStocks));

            CreateMap<CloudStockResponseModel, CloudStock>()
                .ReverseMap();

            CreateMap<TraderCloudStockResponseModel, TraderCloudStock>()
                .ReverseMap();
        }
    }
}
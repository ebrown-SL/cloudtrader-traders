using AutoMapper;
using CloudTrader.Traders.Models.Api.Response;
using CloudTrader.Traders.Models.POCO;

namespace CloudTrader.Traders.Service
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
        }
    }
}

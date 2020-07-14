using AutoMapper;
using CloudTrader.Traders.Models.Api;
using CloudTrader.Traders.Models.Data;
using CloudTrader.Traders.Models.Service;

namespace CloudTrader.Traders.Service
{
    public class TraderProfile : Profile
    {
        public TraderProfile()
        {
            CreateMap<Trader, TraderDbModel>()
                .ReverseMap();

            CreateMap<TraderResponseModel, TraderDbModel>()
                .ReverseMap();

            CreateMap<TraderDbModel, GetTraderMinesResponseModel>()
                .ForMember(dest => dest.TraderId, act => act.MapFrom(src => src.Id));
        }
    }
}

using AutoMapper;
using MagicVilla_CoupounAPI.Models;
using MagicVilla_CoupounAPI.Models.DTO;

namespace MagicVilla_CoupounAPI
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            CreateMap<Coupon,CouponCreateDTO>().ReverseMap();
            CreateMap<Coupon,CouponDTO>().ReverseMap();
            CreateMap<Coupon,CouponEditedDTO>().ReverseMap();

        }

    }
}

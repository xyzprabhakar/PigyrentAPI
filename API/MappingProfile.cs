using API.Models;
using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Routing.Constraints;
using ProductServices;
using StaticContentServices;
using UserDetail;

namespace API
{
    public class MappingProfile:Profile
    {
        public MappingProfile() 
        {
            var currentDt= DateTime.Now;

            CreateMap<DateTime, Google.Protobuf.WellKnownTypes.Timestamp>()
                .ConvertUsing(x => Timestamp.FromDateTime(DateTime.SpecifyKind(x, DateTimeKind.Utc)));
            CreateMap<Google.Protobuf.WellKnownTypes.Timestamp, DateTime>()
                .ConvertUsing(x => x.ToDateTime());

            CreateMap<mdlCategoryDetails, dtoCategoryDetail>().ReverseMap();                
            CreateMap<mdlCategory, dtoCategory>().ReverseMap()
                .ForMember(x => x.CategoryId, opts => opts.PreCondition((src) => src.CategoryId != null))                
                .ForMember(dest => dest.ModifiedDt, act => act.MapFrom(src => Timestamp.FromDateTime(DateTime.SpecifyKind(currentDt, DateTimeKind.Utc))));

            CreateMap<mdlSubCategoryDetails, dtoSubCategoryDetail>().ReverseMap();
            CreateMap<mdlSubCategory, dtoSubCategory>().ReverseMap()
                .ForMember(x => x.SubCategoryId, opts => opts.PreCondition((src) => src.SubCategoryId != null))
                .ForMember(dest => dest.ModifiedDt, act => act.MapFrom(src => Timestamp.FromDateTime(DateTime.SpecifyKind(currentDt, DateTimeKind.Utc))));


            CreateMap<mdlJoinUsDetail, dtoJoinUsDetail>().ReverseMap();
            CreateMap<mdlJoinUs, dtoJoinUs>().ReverseMap()
                .ForMember(x => x.JoinUsId, opts => opts.PreCondition((src) => src.JoinUsId != null))
                .ForMember(dest => dest.ModifiedDt, act => act.MapFrom(src => Timestamp.FromDateTime(DateTime.SpecifyKind(currentDt, DateTimeKind.Utc))));
            CreateMap<mdlJoinUsDetail, dtoJoinUsDetail>().ReverseMap();
            CreateMap<mdlAboutUs, dtoAboutUs>().ReverseMap()
                .ForMember(x => x.AboutUsId, opts => opts.PreCondition((src) => src.AboutUsId != null))
                .ForMember(dest => dest.ModifiedDt, act => act.MapFrom(src => Timestamp.FromDateTime(DateTime.SpecifyKind(currentDt, DateTimeKind.Utc))));

            CreateMap<mdlFAQDetail, dtoFAQDetail>().ReverseMap();
            CreateMap<mdlFAQ, dtoFAQ>().ReverseMap()
                .ForMember(x => x.FAQId, opts => opts.PreCondition((src) => src.FAQId != null))
                .ForMember(dest => dest.ModifiedDt, act => act.MapFrom(src => Timestamp.FromDateTime(DateTime.SpecifyKind(currentDt, DateTimeKind.Utc))));

            CreateMap<mdlLoginRequest, dtoSigninRequest>().ReverseMap();
            CreateMap<mdlLoginResponse, dtoSignInResponse>().ReverseMap();

            CreateMap<mdlLastLoginHistory, dtoLastLoginHistory>().ReverseMap();
            CreateMap<mdlUserLoginDetail, dtoUserLoginDetail>().ReverseMap();
            CreateMap<mdlUserAddress, dtoUserAddress>().ReverseMap();
            CreateMap<mdlUserDetail, dtoUserDetail>().ReverseMap();
            CreateMap<mdlUser, dtoUser>().ReverseMap()
                .ForMember(x => x.UserId, opts => opts.PreCondition((src) => src.UserId != null))
                .ForMember(dest => dest.ModifiedDt, act => act.MapFrom(src => Timestamp.FromDateTime(DateTime.SpecifyKind(currentDt, DateTimeKind.Utc))));

        }
    }
}

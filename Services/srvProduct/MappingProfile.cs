using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using srvProduct.DB;
using srvProduct.protos;

namespace srvProduct
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<DateTime, Google.Protobuf.WellKnownTypes.Timestamp>()
                .ConvertUsing(x => Timestamp.FromDateTime(DateTime.SpecifyKind(x, DateTimeKind.Utc)));
            CreateMap<Google.Protobuf.WellKnownTypes.Timestamp, DateTime>()
                .ConvertUsing(x => x.ToDateTime());
            
            CreateMap<mdlProperty, tblProperty>().ReverseMap();
            CreateMap<mdlCategoryDetails,tblCategoryDetail>().ReverseMap();
            CreateMap<mdlCategory,tblCategoryMaster>().ReverseMap();
            CreateMap<mdlSubCategoryDetails,tblSubCategoryDetail>().ReverseMap()
                .ForMember(x => x.Properties, opts => opts.PreCondition((src) => src.Properties != null)); ;
            CreateMap<mdlSubCategory, tblSubCategoryMaster>().ReverseMap();
            

        }
    }
}

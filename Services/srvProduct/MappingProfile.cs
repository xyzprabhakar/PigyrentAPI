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
            CreateMap<mdlcategoryProperty, tblCategoryProperty>().ReverseMap();
            CreateMap<mdlCategory, tblCategory>().ReverseMap()
                .ForMember(x => x.CategoryProperties , opts => opts.PreCondition((src) => src.CategoryProperties != null));
                
        }
    }
}

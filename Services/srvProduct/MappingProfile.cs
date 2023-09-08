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

            CreateMap<string, Guid>().ConvertUsing(s => string.IsNullOrWhiteSpace(s) ? Guid.Empty : Guid.Parse(s));
            CreateMap<string, Guid?>().ConvertUsing(s => string.IsNullOrWhiteSpace(s) ? (Guid?)null : Guid.Parse(s));
            CreateMap<Guid?, string>().ConvertUsing(g => g.HasValue?g.Value.ToString("N"):string.Empty);
            CreateMap<Guid, string>().ConvertUsing(g => g.ToString("N"));

            CreateMap<mdlProperty, tblProperty>().ForMember(x=>x.Options, y => y.MapFrom(a => string.Join(",", a.Option)));
            CreateMap<tblProperty,mdlProperty >().ForMember(x => x.Option, y => y.MapFrom(a => a.Options!.Split(",", StringSplitOptions.RemoveEmptyEntries)));

            CreateMap<mdlCategoryDetails,tblCategoryDetail>().ReverseMap();
            CreateMap<mdlCategory,tblCategoryMaster>().ReverseMap();
            CreateMap<mdlSubCategoryDetails, tblSubCategoryDetail>()
                .ForMember(x => x.Keywords, y => y.MapFrom(a => string.Join(",", a.Keywords)));
            //.ForMember(x => x.Properties, opts => opts.PreCondition((src) => src.Properties != null)); 
            CreateMap<tblSubCategoryDetail,mdlSubCategoryDetails >()
            .ForMember(x => x.Keywords, y => y.MapFrom(a =>a.Keywords.Split(",",StringSplitOptions.RemoveEmptyEntries)));
            CreateMap<mdlSubCategory, tblSubCategoryMaster>().ReverseMap();
            

        }
    }
}

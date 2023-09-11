using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using srvStaticWeb.DB;

namespace srvStaticWeb
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<DateTime, Google.Protobuf.WellKnownTypes.Timestamp>()
                .ConvertUsing(x => Timestamp.FromDateTime(DateTime.SpecifyKind(x, DateTimeKind.Utc)));
            CreateMap<Google.Protobuf.WellKnownTypes.Timestamp, DateTime>()
                .ConvertUsing(x => x.ToDateTime());

            CreateMap<string, Guid>().ConvertUsing(s => string.IsNullOrWhiteSpace(s) ? Guid.Empty : Guid.Parse(s));
            CreateMap<string, Guid?>().ConvertUsing(s => string.IsNullOrWhiteSpace(s) ? (Guid?)null : Guid.Parse(s));
            CreateMap<Guid?, string>().ConvertUsing(g => g.HasValue ? g.Value.ToString("N") : string.Empty);
            CreateMap<Guid, string>().ConvertUsing(g => g.ToString("N"));

            CreateMap<mdlAboutUsDetail, tblAboutUsDetail>().ReverseMap();
            CreateMap<mdlAboutUs, tblAboutUsMaster>().ReverseMap();

            CreateMap<mdlJoinUsDetail, tblJoinUsDetail>().ReverseMap();
            CreateMap<mdlJoinUs, tblJoinUsMaster>().ReverseMap();

            CreateMap<mdlFAQDetail, tblFAQDetail>().ReverseMap();
            CreateMap<mdlFAQ, tblFAQMaster>().ReverseMap();
        }
    }
}

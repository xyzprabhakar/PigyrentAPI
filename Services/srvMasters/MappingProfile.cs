using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using srvMasters.DB;
using srvMasters.protos;

namespace srvMasters
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DateTime, Google.Protobuf.WellKnownTypes.Timestamp>()
                .ConvertUsing(x => Timestamp.FromDateTime(DateTime.SpecifyKind(x, DateTimeKind.Utc)));
            CreateMap<Google.Protobuf.WellKnownTypes.Timestamp, DateTime>()
                .ConvertUsing(x => x.ToDateTime());
            CreateMap<mdlCurrency, tblCurrency>().ReverseMap();
        }
    }
}

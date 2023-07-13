using AutoMapper;
using MasterServices.DB;
using MasterServicesProt;

namespace MasterServices
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<State, tblState>();
            CreateMap<Country, tblCountry>();
            CreateMap<mdlCurrency, tblCurrency>();
        }
    }
}

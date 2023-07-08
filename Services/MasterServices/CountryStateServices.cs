using DTO;
using MasterServicesProt;
using ProtoBuf.Grpc;

namespace MasterServices
{
    public class CountryStateServices : ICountryStateService
    {
        public Task<ReturnList<State>> GetAllState(CallContext context = default)
        {
            throw new NotImplementedException();
        }

        public Task<ReturnList<Country>> GetCountries(RequestData request, CallContext context = default)
        {
            throw new NotImplementedException();
        }

        public Task<ReturnList<Currency>> GetCountryCurrency(RequestData request, CallContext context = default)
        {
            throw new NotImplementedException();
        }

        public Task<ReturnList<State>> GetCountryStates(RequestData request, CallContext context = default)
        {
            throw new NotImplementedException();
        }

        public Task<State> GetState(RequestData request, CallContext context = default)
        {
            throw new NotImplementedException();
        }

        public Task<ReturnList<CountryTopCity>> GetTopCities(RequestData request, CallContext context = default)
        {
            throw new NotImplementedException();
        }

        public Task<ReturnData> SaveCountry(Country request, CallContext context = default)
        {
            throw new NotImplementedException();
        }

        public Task<ReturnData> SaveState(State request, CallContext context = default)
        {
            throw new NotImplementedException();
        }
    }
}

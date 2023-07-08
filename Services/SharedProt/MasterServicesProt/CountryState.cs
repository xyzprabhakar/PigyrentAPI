using DTO;
using ProtoBuf.Grpc;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace MasterServicesProt
{
    [DataContract]
    public class Country
    {
        [DataMember(Order = 1)]
        public string CountryId { get; set; } = null!;
        [Required]
        [DataMember(Order = 2)]
        public string Code { get; set; } = null!;
        [Required]
        [DataMember(Order = 3)]
        public string Name { get; set; } = null!;        
        [DataMember(Order = 4)]
        public string? PhoneCode { get; set; }
        [DataMember(Order = 5)]
        public string? Capital { get; set; }
        [DataMember(Order = 6)]
        public List<CountryTopCity>? Cities { get; set; }
        [DataMember(Order = 7)]
        public List<string>? Currency { get; set; }
        [DataMember(Order = 8)]
        public List<string>? Languages { get; set; }
        [DataMember(Order = 9)]
        public List<CountryTimeZone>? TimeZones { get; set; }
        [DataMember(Order = 10)]
        public bool IsActive { get; set; }
        [DataMember(Order = 11)]
        public string? ModifiedBy { get; set; }
        [DataMember(Order = 12)]
        public DateTime ModifiedDt { get; set; }


    }
    [DataContract]
    public class CountryTopCity
    {
        [DataMember(Order = 1)]
        public string CityName { get; set; } = null!;
        [DataMember(Order = 2)]
        public string? StateId { get; set; } 
        [DataMember(Order = 3)]
        public int ProductCount { get; set; } 
    }
    [DataContract]
    public class CountryTimeZone
    {
        [DataMember(Order = 1)]
        public string ZoneName { get; set; } = null!;
        [DataMember(Order = 2)]
        public string? Offset { get; set; }
        [DataMember(Order = 3)]
        public string? OffsetName { get; set; }
        [DataMember(Order = 4)]
        public string? Abbreviation { get; set; }
        [DataMember(Order = 5)]
        public string? TzName { get; set; }
    }
    [DataContract]
    public class State
    {
        [DataMember(Order = 1)]
        public string StateId { get; set; } = null!;
        [DataMember(Order = 2)]
        public string Code { get; set; } = null!;
        [DataMember(Order = 3)]
        public string Name { get; set; } = null!;
        [DataMember(Order = 4)]
        public string CountryId { get; set; } = null!;
        [DataMember(Order = 5)]
        public string? Latitude{ get; set; }
        [DataMember(Order = 6)]
        public string? Longitude { get; set; }
        [DataMember(Order = 7)]
        public List<City>? Cities { get; set; }
    }
    [DataContract]
    public class City
    {
        [DataMember(Order = 1)]
        public string CityName { get; set; } = null!;        
        [DataMember(Order = 2)]
        public int ProductCount { get; set; }
        [DataMember(Order = 3)]
        public string? Latitude { get; set; }
        [DataMember(Order = 4)]
        public string? Longitude { get; set; }
    }

    [ServiceContract]
    public interface ICountryStateService
    {
        [OperationContract]
        Task<ReturnData> SaveCountry(Country request, CallContext context = default);
        [OperationContract]
        Task<ReturnData> SaveState(State request, CallContext context = default);
        [OperationContract]
        Task<ReturnList<Country>> GetCountries(RequestData request, CallContext context = default);
        [OperationContract]
        Task<ReturnList<CountryTopCity>> GetTopCities(RequestData request, CallContext context = default);
        [OperationContract]
        Task<ReturnList<string>> GetCountryCurrency(RequestData request, CallContext context = default);                
        [OperationContract]
        Task<ReturnList<State>> GetAllState( CallContext context = default);
        [OperationContract]
        Task<ReturnList<State>> GetCountryStates(RequestData request, CallContext context = default);
        [OperationContract]
        Task<State> GetState(RequestData request, CallContext context = default);
    }
}
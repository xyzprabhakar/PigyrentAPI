using MasterServices;

namespace API.Models
{
    
    

    public class dtoCountry : ModifyDetails
    {
        public string CountryId { get; set; } = null!;
        public string Code { get; set; } = null!;
        public int DisplayOrder { get; set; }
        public List<dtoCountryDetail> CountryDetail { get; set; } = new();
        public List<dtoTimeZone> TimeZones { get; set; } = new();
    }
    public class dtoTimeZone
    {
        public string? ZoneName { get; set; }
        public string? Offset { get; set; }
        public string? OffsetName { get; set; }
        public string? Abbreviation { get; set; }
        public string? TzName { get; set; }
    }
    public class dtoCountryDetail: BasicDetails
    {
        
        public string? PhoneCode { get; set; }
        public string? Capital { get; set; }
        public string? StateSynonyms { get; set; }        
        public bool HaveState { get; set; }
        public List<string> Languages { get; set; } = new();
        public List<string> Currency { get; set; } = new();
    }

    public class dtoState : ModifyDetails
    {
        public string StateId { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string CountryId { get; set; } = string.Empty;
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public int DisplayOrder { get; set; }
        public List<dtoStateDetail> StateDetail { get; set; } = new();
    }
    public class dtoStateDetail : BasicDetails
    {

    }

    public class dtoCity : ModifyDetails
    {
        public string CityId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string CountryId { get; set; } = string.Empty;
        public string StateId { get; set; } = string.Empty;
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public List<dtoCityDetail> CityDetail { get; set; } = new();
    }

    public class dtoCityDetail : BasicDetails
    {
    }

    public class dtoLocality : ModifyDetails
    {
        public string LocalityId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string CityId { get; set; } = string.Empty;
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public List<dtoLocalityDetail> LocalityDetail { get; set; } = new();
    }
    public class dtoLocalityDetail : BasicDetails
    {
    }
    public class dtoCurrency : ModifyDetails
    {
        public string CurrencyId { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
        public List<dtoLocalityDetail> CurrencyDetail { get; set; } = new();
    }

    public class dtoCurrencyDetail : BasicDetails
    {
    }

    public class dtoLanguage : ModifyDetails
    {
        public string LanguageId { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Name { get; set; }

    }

    
}

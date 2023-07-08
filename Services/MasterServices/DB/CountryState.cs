using MasterServicesProt;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace MasterServices.DB
{
    public class tblCountry
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? CountryId { get; set; }
        public string Code { get; set; } = null!;        
        public string Name { get; set; } = null!;        
        public string? PhoneCode { get; set; }        
        public string? Capital { get; set; }        
        public List<CountryTopCity>? Cities { get; set; }        
        public List<string>? Currency { get; set; }        
        public List<string>? Languages { get; set; }        
        public List<CountryTimeZone>? TimeZones { get; set; }        
        public bool IsActive { get; set; }        
        public string? ModifiedBy { get; set; }        
        public DateTime ModifiedDt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDt { get; set; }
    }

    public class tblState
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string StateId { get; set; } = null!;        
        public string Code { get; set; } = null!;        
        public string Name { get; set; } = null!;        
        public string CountryId { get; set; } = null!;        
        public string? Latitude { get; set; }        
        public string? Longitude { get; set; }        
        public List<City>? Cities { get; set; }
        public bool IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime ModifiedDt { get; set; }        
    }
}

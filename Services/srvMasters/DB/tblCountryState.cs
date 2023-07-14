using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using srvMasters.protos;

namespace srvMasters.DB
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
        public List<mdlCountryTopCity>? Cities { get; set; }
        public List<string>? Currency { get; set; }
        public List<string>? Languages { get; set; }
        public List<mdlCountryTimeZone>? TimeZones { get; set; }
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
        public List<mdlCity>? Cities { get; set; }
        public bool IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime ModifiedDt { get; set; }
    }
}

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Runtime.Serialization;

namespace MasterServices.DB
{
    public class tblCurrency
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CurrencyId { get; set; } = null!;        
        public string Code { get; set; } = null!;        
        public string Name { get; set; } = null!;
        public string Symbol { get; set; } = null!;
        public string? CreatedBy { get; set; }
        public DateTime CreatedDt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime ModifiedDt { get; set; }
    }
}

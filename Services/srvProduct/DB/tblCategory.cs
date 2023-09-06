using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using srvProduct.protos;
using MongoDB.Driver;

namespace srvProduct.DB
{
    public class tblCategoryMaster
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? CategoryId { get; set; }
        public string DefaultName { get; set; } = null!;
        public bool IsActive { get; set; }
        public string? ImageUrl { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime ModifiedDt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDt { get; set; }
    }

    public  abstract class BasicDetails
    {
        public string Language { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? ShortDesc { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime ModifiedDt { get; set; }
    }

    public class tblCategoryDetail: BasicDetails
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? CategoryDetailId { get; set; }        
        public string? CategoryId { get; set; }
    }
    public class tblProperty
    {
        public int PropertyDisplayOrder { get; set; }
        public string Name { get; set; } = null!;
        public enmPropertyType Type { get; set; }
        public int MinLength { get; set; }
        public int MaxLength { get; set; }
        public string? Regx { get; set; }
        public List<string>? Option { get; set; }
    }

    public class tblSubCategoryMaster
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? SubCategoryId { get; set; }        
        public string? CategoryId { get; set; } 
        public string DefaultName { get; set; } = null!;
        public bool IsActive { get; set; }
        public string? ImageUrl { get; set; }        
        public string? ModifiedBy { get; set; }
        public DateTime ModifiedDt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDt { get; set; }
    }

    public class tblSubCategoryDetail : BasicDetails
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? SubCategoryDetailId { get; set; }
        public string? SubCategoryId { get; set; } 
        public List<string> Keywords { get; set; } = null!;
        public List<tblProperty> Properties { get; set; } = null!;
    }

}

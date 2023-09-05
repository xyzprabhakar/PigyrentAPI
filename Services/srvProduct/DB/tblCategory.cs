using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using srvProduct.protos;

namespace srvProduct.DB
{
    public  abstract class BasicDetails
    {
        public string Language { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? ShortDesc { get; set; }
    }

    public class tblCategory
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? CategoryId { get; set; }
        public bool IsActive { get; set; }     
        public string? ImageUrl { get; set; }
        public List<tblCategoryDetail> CategoryDetail { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime ModifiedDt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDt { get; set; }
    }
    public class tblCategoryDetail: BasicDetails
    {   
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

    public class tblSubCategory
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? SubCategoryId { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string? CategoryId { get; set; }
        public bool IsActive { get; set; }
        public string? ImageUrl { get; set; }
        public List<tblSubCategoryDetails> SubCategoryDetail { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime ModifiedDt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDt { get; set; }
    }

    public class tblSubCategoryDetails : BasicDetails
    {
        public List<string> Keywords { get; set; } = null!;
        public List<tblProperty> Properties { get; set; } = null!;
    }

}

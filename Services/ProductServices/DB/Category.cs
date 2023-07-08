using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using ProductServicesProt;
using System.Runtime.Serialization;
using DTO;

namespace ProductServices.DB
{

    public class tblCategory
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? CategoryId { get; set; } 
        public bool IsActive { get; set; }
        public string? ParentCategoryId { get; set; }
        public string? RootCategoryId { get; set; }
        public string Title { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? ShortDesc { get; set; }
        public List<string> Keywords { get; set; } = null!;
        public List<tblCategoryProperty> CategoryProperties { get; set; } = null!;        
        public string? ModifiedBy { get; set; }        
        public DateTime ModifiedDt { get; set; }        
        public string? CreatedBy { get; set; }        
        public DateTime CreatedDt { get; set; }
    }
    public class tblCategoryProperty
    {   
        public int PropertyDisplayOrder { get; set; }        
        public string Name { get; set; }=null!;        
        public PropertyType Type { get; set; }        
        public int MinLength { get; set; }        
        public int MaxLength { get; set; }        
        public string? Regx { get; set; }        
        public List<string>? Option { get; set; }
    }

}

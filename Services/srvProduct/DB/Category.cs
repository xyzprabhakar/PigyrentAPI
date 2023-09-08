using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using srvProduct.protos;
using MongoDB.Driver;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace srvProduct.DB
{
    public abstract class BasicDetails
    {
        [MaxLength(32)]
        public string Language { get; set; } = null!;
        [MaxLength(64)]
        public string Title { get; set; } = null!;
        [MaxLength(64)]
        public string Name { get; set; } = null!;
        [MaxLength(256)]
        public string? ShortDesc { get; set; }
        [MaxLength(64)]
        public string? ModifiedBy { get; set; }
        public DateTime ModifiedDt { get; set; }
        public bool IsDeleted { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDt { get; set; }
    }

    public class tblCategoryMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CategoryId { get; set; }
        [MaxLength(32)]
        public string DefaultName { get; set; } = null!;
        public bool IsActive { get; set; }
        [MaxLength(256)]
        public string? ImageUrl { get; set; }
        [MaxLength(64)]
        public string? ModifiedBy { get; set; }
        public DateTime ModifiedDt { get; set; }
        [MaxLength(64)]
        public string? CreatedBy { get; set; }
        public DateTime CreatedDt { get; set; }
        public virtual ICollection<tblCategoryDetail>? CategoryDetail { get; set; }
    }
    public class tblCategoryDetail: BasicDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CategoryDetailId { get; set; }
        [ForeignKey("tblCategoryMaster")]
        public Guid? CategoryId { get; set; }
        public tblCategoryMaster? tblCategoryMaster { get; set; }

    }
    

    public class tblSubCategoryMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid SubCategoryId { get; set; }
        [ForeignKey("tblCategoryMaster")]
        public Guid? CategoryId { get; set; }
        public tblCategoryMaster? tblCategoryMaster { get; set; }
        public string DefaultName { get; set; } = null!;

        public bool IsActive { get; set; }
        [MaxLength(256)]
        public string? ImageUrl { get; set; }
        [MaxLength(64)]
        public string? ModifiedBy { get; set; }
        public DateTime ModifiedDt { get; set; }
        [MaxLength(64)]
        public string? CreatedBy { get; set; }
        public DateTime CreatedDt { get; set; }
        public virtual ICollection<tblSubCategoryDetail>? SubCategoryDetail { get; set; }

    }

    public class tblSubCategoryDetail : BasicDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid SubCategoryDetailId { get; set; }
        [ForeignKey("tblSubCategoryMaster")]
        public Guid? SubCategoryId { get; set; }
        public tblSubCategoryMaster? tblSubCategoryMaster { get; set; }
        public string Keywords { get; set; } = null!;
        public virtual ICollection<tblProperty>? Properties { get; set; } 
    }
    public class tblProperty
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid PropertyId { get; set; }
        [ForeignKey("tblSubCategoryDetail")]
        public Guid? SubCategoryDetailId { get; set; }
        public tblSubCategoryDetail? tblSubCategoryDetail { get; set; }
        public int PropertyDisplayOrder { get; set; }
        [MaxLength(64)]
        public string Name { get; set; } = null!;
        public enmPropertyType Type { get; set; }
        public int MinLength { get; set; }
        public int MaxLength { get; set; }
        [MaxLength(128)]
        public string? Regx { get; set; }
        [MaxLength(258)]
        public string? Options { get; set; }
        public bool IsDeleted { get; set; }
    }

}

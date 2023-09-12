using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace srvStaticWeb.DB
{
    public class tblContactUs :clsModifiedBy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ContactUsId { get; set; }
        [MaxLength(32)]
        public string Name { get; set; } = null!;
        [MaxLength(128)]
        public string Email { get; set; } = null!;
        [MaxLength(128)]
        public string Subject { get; set; } = null!;
        [MaxLength(512)]
        public string Messages { get; set; } = null!;
    }
    public class tblOffice :clsModifiedBy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid OfficeId { get; set; }
        public bool IsActive { get; set; }
        public bool IsHeadOffice { get; set; }
        [MaxLength(32)]
        public string DefaultLocation { get; set; } = null!;
        public virtual ICollection<tblOfficeDetail>? OfficeDetail { get; set; }
    }
    public class tblOfficeDetail : clsModifiedBy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid OfficeDetailId { get; set; }
        [MaxLength(32)]
        public string Region { get; set; }=null!;
        [MaxLength(32)]
        public string Location { get; set; }=null!;
        [MaxLength(256)]
        public string Address { get; set; } = null!;
        [MaxLength(32)]
        public string? ContactNo { get; set; }
        [MaxLength(32)]
        public string? Longitude { get; set; }
        [MaxLength(32)]
        public string? Latitude { get; set; }
        [MaxLength(256)]
        public string? Image { get; set; }
        [MaxLength(128)]
        public string? Email { get; set; }
        [MaxLength(32)]
        public string Language { get; set; } = null!;
        public bool IsDeleted { get; set; }
        [ForeignKey("tblOffice")]
        public Guid? OfficeId { get; set; }
        public tblOffice? tblOffice { get; set; }
    }


}

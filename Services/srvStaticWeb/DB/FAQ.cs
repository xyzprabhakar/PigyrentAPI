using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace srvStaticWeb.DB
{
    public class tblFAQMaster: clsModifiedBy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid FAQId { get; set; }
        [MaxLength(32)]
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(256)]
        public string DefaultQuestion { get; set; } = null!;        
        public virtual ICollection<tblFAQDetail>? FAQDetail { get; set; }
    }
    public class tblFAQDetail : clsModifiedBy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid FAQDetailId { get; set; }
        [MaxLength(256)]
        public string Question { get; set; } = null!;
        [MaxLength(1024)]
        public string Answer { get; set; } = null!;
        [MaxLength(32)]
        public string Language { get; set; } = null!;
        [ForeignKey("tblFAQMaster")]
        public Guid? FAQId { get; set; }
        public tblFAQMaster? tblFAQMaster { get; set; }
        public bool IsDeleted { get; set; }
    }
}

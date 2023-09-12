using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using srvStaticWeb.protos;

namespace srvStaticWeb.DB
{
    public class tblComplaintMaster:clsModifiedBy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ComplaintId { get; set; }
        [MaxLength(32)]        
        public string ComplaintNo { get; set; } = null!;
        [MaxLength(32)]
        public string Name { get; set; } = null!;
        [MaxLength(128)]
        public string Email { get; set; } = null!;
        [MaxLength(32)]
        public string? ContactNo { get; set; } 
        [MaxLength(128)]
        public string Subject { get; set; } = null!;
        [MaxLength(512)]
        public string Messages { get; set; } = null!;
        [MaxLength(128)]
        public string? FilePath { get; set; }
        public enmComplainType ComplaintType { get; set; }
        public enmComplaintStatus ComplaintStatus { get; set; }

        public bool IsDeleted { get; set; }
        public virtual ICollection<tblComplaintProcess>? ComplaintProcess { get; set; }

    }
    public class tblComplaintProcess : clsModifiedBy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ComplaintProcessId { get; set; }
        [MaxLength(512)]
        public string Messages { get; set; } = null!;
        [MaxLength(32)]
        public string? ProcessBy { get; set; }
        [ForeignKey("tblComplaintMaster")]
        public Guid? ComplaintId { get; set; }
        public tblComplaintMaster? tblComplaintMaster { get; set; }
        public enmComplaintStatus ComplaintStatus { get; set; }
    }
}

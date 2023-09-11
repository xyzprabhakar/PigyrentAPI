using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using srvStaticWeb;
namespace srvStaticWeb.DB
{
    public class tblComplaintMaster:clsModifiedBy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid OfficeDetailId { get; set; }
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
        //public  enmComplainType ComplainType { get; set; }
        public bool IsDeleted { get; set; }

    }
}

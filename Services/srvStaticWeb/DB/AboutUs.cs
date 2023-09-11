using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace srvStaticWeb.DB
{

    public class tblAboutUsMaster : clsModifiedBy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AboutUsId { get; set; }
        [MaxLength(32)]
        public string DefaultName { get; set; } = null!;
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
        public virtual ICollection<tblAboutUsDetail>? AboutUsDetail { get; set; }
    }

    public class tblAboutUsDetail : clsAboutUsDetailData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AboutUsDetailId { get; set; }
        [ForeignKey("tblAboutUsMaster")]
        public Guid? AboutUsId { get; set; }
        public tblAboutUsMaster? tblAboutUsMaster { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class tblJoinUsMaster : clsModifiedBy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid JoinUsId { get; set; }
        [MaxLength(32)]
        public string DefaultName { get; set; } = null!;
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
        public virtual ICollection<tblJoinUsDetail>? JoinUsDetail { get; set; }
    }

    public class tblJoinUsDetail : clsAboutUsDetailData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid JoinUsDetailId { get; set; }
        [ForeignKey("tblJoinUsMaster")]
        public Guid? JoinUsId { get; set; }
        public tblJoinUsMaster? tblJoinUsMaster { get; set; }
        public bool IsDeleted { get; set; }
    }



}

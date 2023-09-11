using System.ComponentModel.DataAnnotations;

namespace srvStaticWeb.DB
{
    public abstract class clsModifiedBy
    {
        [MaxLength(64)]
        public string? ModifiedBy { get; set; }
        public DateTime ModifiedDt { get; set; }
        [MaxLength(64)]
        public string? CreatedBy { get; set; }
        public DateTime CreatedDt { get; set; }
    }
    public abstract class clsAboutUsDetailData: clsModifiedBy
    {
        [MaxLength(64)]
        public string Title { get; set; } = null!;
        [MaxLength(32)]
        public string Language { get; set; } = null!;
        [MaxLength(64)]
        public string Heading { get; set; } = null!;
        [MaxLength(2048)]
        public string Details { get; set; } = null!;
    }

}

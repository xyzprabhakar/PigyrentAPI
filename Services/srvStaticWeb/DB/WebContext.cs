using Microsoft.EntityFrameworkCore;

namespace srvStaticWeb.DB
{
    public class WebContext : DbContext
    {
        public WebContext(DbContextOptions<WebContext> options) : base(options)
        {
        }

        public DbSet<tblAboutUsMaster> tblAboutUsMaster { get; set; }
        public DbSet<tblAboutUsDetail> tblAboutUsDetail { get; set; }
        public DbSet<tblJoinUsMaster> tblJoinUsMaster { get; set; }
        public DbSet<tblJoinUsDetail> tblJoinUsDetail { get; set; }
        public DbSet<tblFAQMaster> tblFAQMaster { get; set; }
        public DbSet<tblFAQDetail> tblFAQDetail { get; set; }

        public DbSet<tblContactUs> tblContactUs { get; set; }
        public DbSet<tblOffice> tblOffice { get; set; }
        public DbSet<tblOfficeDetail> tblOfficeDetail { get; set; }

        public DbSet<tblComplaintMaster> tblComplaintMaster { get; set; }
        public DbSet<tblComplaintProcess> tblComplaintProcess { get; set; }


    }
}

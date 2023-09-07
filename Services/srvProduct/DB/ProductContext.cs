using Microsoft.EntityFrameworkCore;

namespace srvProduct.DB
{
    public class ProductContext:DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options):base(options)
        { 
        }

        public DbSet<tblCategoryMaster> tblCategoryMaster { get; set; }
        public DbSet<tblCategoryDetail> tblCategoryDetail { get; set; }
        public DbSet<tblSubCategoryMaster> tblSubCategoryMaster { get; set; }
        public DbSet<tblSubCategoryDetail> tblSubCategoryDetail { get; set; }
        public DbSet<tblProperty> tblProperty { get; set; }
    }
}

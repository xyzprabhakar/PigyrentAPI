namespace srvProduct.DB
{
    public class DbSetting
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string CategoryMasterCollection { get; set; } = null!;
        public string CategoryDetailCollection { get; set; } = null!;
        public string SubCategoryMasterCollection { get; set; } = null!;
        public string SubCategoryDetailCollection { get; set; } = null!;
        public int IdLength { get; set; }
    }
}

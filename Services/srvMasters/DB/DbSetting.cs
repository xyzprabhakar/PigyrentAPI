namespace srvMasters.DB
{
    public class DbSetting
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string CountryCollection { get; set; } = null!;
        public string StateCollection { get; set; } = null!;
        public string CurrencyCollection { get; set; } = null!;
        public int IdLength { get; set; }
    }
}

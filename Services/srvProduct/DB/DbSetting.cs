﻿namespace srvProduct.DB
{
    public class DbSetting
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string CategoryCollection { get; set; } = null!;
        public int IdLength { get; set; }
    }
}

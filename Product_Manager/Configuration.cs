namespace Product_Manager
{
    static class Configuration
    {
        public static IConfiguration AppSetting
        {
            get;
        }
        static Configuration()
        {
            AppSetting = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
        }
    }
}

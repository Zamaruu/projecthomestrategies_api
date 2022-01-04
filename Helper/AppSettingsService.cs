using System.IO;
using Microsoft.Extensions.Configuration;

namespace HomeStrategiesApi.Helper
{
    public class AppSettingsService
    {
        public static IConfiguration AppSetting { get; set; }

        static AppSettingsService()
        {
            AppSetting = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();

        }

    }
}
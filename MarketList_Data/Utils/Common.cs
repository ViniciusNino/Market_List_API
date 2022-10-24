using System;
using Microsoft.Extensions.Configuration;

namespace MarketList_API.Data
{
    public static class Common
    {
        private static IConfigurationSection settings => new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings");
        private static IConfigurationSection mailSettings => new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("MailSettings");
        private static IConfigurationSection jwtSettings => new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtSettings");

        public static string GetSettings(string variable)
        {
            var env = Environment.GetEnvironmentVariable(variable) ?? settings[variable];
            return env;
        }

        public static string GetMailSettings(string variable)
        {
            var env = Environment.GetEnvironmentVariable(variable) ?? mailSettings[variable];
            return env;
        }

        public static string GetJwtSettings(string variable)
        {
            var env = Environment.GetEnvironmentVariable(variable) ?? jwtSettings[variable];
            return env;
        }

        public static string GetApplicationUrl()
        {
            if (Convert.ToBoolean(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()["UseDev"]))
                return mailSettings["ApplicationUrlDev"];

            var env = Environment.GetEnvironmentVariable("ApplicationUrl") ?? mailSettings["ApplicationUrl"];
            return env;
        }
    }
}
using System;
using Microsoft.Extensions.Configuration;

namespace MedPlannerCore.Data.Utils
{
    public static class Common
    {
        private static IConfigurationSection settings => new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings");

        public static string GetSettings(string variable)
        {
            return Environment.GetEnvironmentVariable(variable) ?? settings[variable];
        }
    }
}